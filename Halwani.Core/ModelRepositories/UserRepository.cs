﻿using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Halwani.Data.Entities.User;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.UserModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;

namespace Halwani.Core.ModelRepositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public IEnumerable<UserLookupViewModel> ListItPersonal(string teamName)
        {

            try
            {
                return Find(s => s.UserTeams.Any(t => t.Team.Name == teamName && t.User.RoleId == (int)RoleEnum.ItPersonal)).Select(e => new UserLookupViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    UserName = e.UserName,
                    Email = e.Email
                }).ToList();

            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }

        }

        public IEnumerable<UserLookupViewModel> ListReporters(ClaimsIdentity claimsIdentity)
        {
            try
            {
                var teams = claimsIdentity.Claims.FirstOrDefault(e => e.Type == AdditionalClaims.Teams).Value;

                return Find(s => s.UserTeams.Any(ut => teams.Contains(ut.Team.Name)) && s.RoleId == (int)RoleEnum.User).Select(e => new UserLookupViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    Email = e.Email,
                    UserName = e.UserName,
                    Team = string.Join(",", e.UserTeams.Select(e => e.Team.Name))
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput ReadUsersExcel(IFormFile attachement)
        {
            try
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(attachement.OpenReadStream(), false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    StringBuilder excelResult = new StringBuilder();

                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                        excelResult.AppendLine("----------------------------------------------- ");
                        //statement to get the worksheet object by using the sheet id  
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                        SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();
                        var firstRow = true;
                        foreach (Row thecurrentrow in thesheetdata)
                        {
                            if (firstRow)
                            {
                                firstRow = false;
                                continue;
                            }
                            var result = ExtractDataFromRow(workbookPart, thecurrentrow, out string nameText, out string emailText, out string userNameText, out string teamIdsText, out RoleEnum securityGroupText);
                            if (result == true)
                            {
                                var checkExistance = Find(e => e.UserName == userNameText || e.Email == emailText).FirstOrDefault();
                                if (checkExistance == null)
                                    Add(new User
                                    {
                                        Email = emailText,
                                        Name = nameText,
                                        UserName = userNameText,
                                        UserStatus = UserStatusEnum.Active,
                                        RoleId = (int)securityGroupText,
                                        UserTeams = teamIdsText.Split(",").Select(e => new Data.Entities.Team.UserTeams
                                        {
                                            TeamId = int.Parse(e)
                                        }).ToList()
                                    });
                            }
                        }
                    }
                }
                Save();
                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse();
            }
        }

        private bool ExtractDataFromRow(WorkbookPart workbookPart, Row thecurrentrow, out string nameText, out string emailText, out string userNameText, out string teamIdsText, out RoleEnum securityGroupText)
        {
            nameText = "";
            emailText = "";
            userNameText = "";
            teamIdsText = "";
            securityGroupText = RoleEnum.User;
            var name = thecurrentrow.ChildElements.ElementAt(0);
            int id;
            if (Int32.TryParse(name.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                nameText = item.Text.InnerText.ToString().Trim();
                if (string.IsNullOrEmpty(nameText))
                    return false;
            }
            var email = thecurrentrow.ChildElements.ElementAt(1);
            if (Int32.TryParse(email.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                emailText = item.Text.InnerText.ToString().Trim();
                if (string.IsNullOrEmpty(emailText))
                    return false;
            }
            var userName = thecurrentrow.ChildElements.ElementAt(2);
            if (Int32.TryParse(userName.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                userNameText = item.Text.InnerText.ToString().Trim();
                if (string.IsNullOrEmpty(userNameText))
                    return false;
            }
            var teamIds = thecurrentrow.ChildElements.ElementAt(3);
            if (Int32.TryParse(teamIds.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                teamIdsText = item.Text.InnerText.ToString().Trim();
                if (!string.IsNullOrEmpty(teamIdsText))
                {
                    foreach (var team in teamIdsText.Split(","))
                    {
                        if (!int.TryParse(team, out int teamId))
                        {
                            teamIdsText = "";
                            break;
                        }
                    }
                }
            }
            var securityGroup = thecurrentrow.ChildElements.ElementAt(4);
            if (int.TryParse(securityGroup.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                if (item.Text.InnerText.ToString().ToLower().Contains("it- admin"))
                    securityGroupText = RoleEnum.ItManager;
                if (item.Text.InnerText.ToString().Contains("it-user"))
                    securityGroupText = RoleEnum.ItPersonal;
                if (item.Text.InnerText.ToString().Contains("user"))
                    securityGroupText = RoleEnum.User;
            }
            return true;
        }

        //public RepositoryOutput Add(List<CreateUserModel> model)
        //{
        //    try
        //    {
        //        AddRange(model.Select(item => new User()
        //        {
        //            Name = item.Name
        //        }).ToList());

        //        if (Save() < 1)
        //            return RepositoryOutput.CreateErrorResponse("");
        //        return RepositoryOutput.CreateSuccessResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        RepositoryHelper.LogException(ex);
        //        return RepositoryOutput.CreateErrorResponse(ex.Message);
        //    }
        //}
    }
}
