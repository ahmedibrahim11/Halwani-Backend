using Halawani.Core;
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
using Halwani.Data.Entities.Incident;

namespace Halwani.Core.ModelRepositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public IEnumerable<UserLookupViewModel> ListItPersonal(string teamName)
        {

            try
            {
                return Find(s => s.UserTeams.Any(t => t.Team.Name == teamName && (t.User.RoleId == (int)RoleEnum.ItPersonal || t.User.RoleId == (int)RoleEnum.ItManager /*|| t.User.RoleId == (int)RoleEnum.SuperAdmin*/))).Select(e => new UserLookupViewModel
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
                return Find(s=>s.RoleId== (int)RoleEnum.User).Select(e => new UserLookupViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    Email = e.Email,
                    UserName = e.UserName,
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

                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection)
                    {
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
                            if (thecurrentrow.ChildElements.Count < 7)
                                continue;
                            var result = ExtractDataFromRow(workbookPart, thecurrentrow, out string nameText, out string emailText, out string userNameText, out List<int> teamIdsText, out RoleEnum securityGroupEnum, out bool priority, out bool isDeleted);
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
                                        RoleId = (int)securityGroupEnum,
                                        SetTicketHigh = priority,
                                        UserTeams = teamIdsText.Select(e => new Data.Entities.Team.UserTeams
                                        {
                                            TeamId = e
                                        }).ToList()
                                    });
                                else
                                {
                                    if (isDeleted)
                                    {
                                        Remove(checkExistance);
                                    }
                                    else
                                    {
                                        checkExistance.Email = emailText;
                                        checkExistance.Name = nameText;
                                        checkExistance.UserName = userNameText;
                                        checkExistance.UserStatus = UserStatusEnum.Active;
                                        checkExistance.RoleId = (int)securityGroupEnum;
                                        checkExistance.SetTicketHigh = priority;
                                        checkExistance.UserTeams = teamIdsText.Select(e => new Data.Entities.Team.UserTeams
                                        {
                                            TeamId = e
                                        }).ToList();
                                    }
                                }
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

        private bool ExtractDataFromRow(WorkbookPart workbookPart, Row thecurrentrow, out string nameText, out string emailText, out string userNameText, out List<int> teamIdsText, out RoleEnum securityGroupText, out bool priority, out bool isDeleted)
        {
            nameText = "";
            emailText = "";
            userNameText = "";
            teamIdsText = new List<int>();
            securityGroupText = RoleEnum.User;
            priority = false;
            isDeleted = false;
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
                var teamText = item.Text.InnerText.ToString().Trim();
                if (!string.IsNullOrEmpty(teamText))
                {
                    foreach (var team in teamText.Split(","))
                    {
                        int teamId;
                        if (!int.TryParse(team, out teamId))
                        {
                            break;
                        }
                        else
                        {
                            teamIdsText.Add(teamId);
                        }
                    }
                }
            }
            var securityGroup = thecurrentrow.ChildElements.ElementAt(4);
            if (int.TryParse(securityGroup.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                if (item.Text.InnerText.ToString().ToLower().Contains("it- admin")|| item.Text.InnerText.ToString().ToLower().Contains("it-admin"))
                    securityGroupText = RoleEnum.ItManager;
                else if (item.Text.InnerText.ToString().ToLower().Contains("it-user"))
                    securityGroupText = RoleEnum.ItPersonal;
                else if(item.Text.InnerText.ToString().ToLower().Contains("user"))
                    securityGroupText = RoleEnum.User;
                else
                    securityGroupText = RoleEnum.User;
            }
            var priorityCell = thecurrentrow.ChildElements.ElementAt(5);
            if (int.TryParse(priorityCell.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                switch (item.Text.InnerText.ToString())
                {
                    case "0":
                        priority = false;
                        break;
                    case "1":
                        priority = true;
                        break;
                    default:
                        priority = false;
                        break;
                }
            }
            var deleteCell = thecurrentrow.ChildElements.ElementAt(6);
            if (int.TryParse(deleteCell.InnerText, out id))
            {
                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                switch (item.Text.InnerText.ToString())
                {
                    case "0":
                        isDeleted = false;
                        break;
                    case "1":
                        isDeleted = true;
                        break;
                    default:
                        isDeleted = false;
                        break;
                }
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
