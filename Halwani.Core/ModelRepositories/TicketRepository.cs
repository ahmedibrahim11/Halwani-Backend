using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.Authentication;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data;
using Halwani.Data.Entities;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Halwani.Core.ModelRepositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
      
        public TicketRepository()
        {
          
        
        }

        public TicketPageResultViewModel List(TicketPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response)
        {
            try
            {
                response = new RepositoryOutput();
                TicketPageResultViewModel result = new TicketPageResultViewModel
                {
                    CanAdd = true
                };

                var qurey = Find(null, null, "");

                qurey = FilterLoggedUser(userClaims, qurey, false);
                qurey = FilterList(model, qurey);
                qurey = SortList(model, qurey);
                PagingList(model, result, qurey);

                return result;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                response = RepositoryOutput.CreateErrorResponse(ex.Message);
                return null;
            }
        }

        public async Task<List<string>> PostFilesAsync(IFormFileCollection attachments, string saveFilePath)
        {
            try
            {
                var result = new List<string>();
                foreach (var file in attachments)
                {
                    var filePath = Path.Combine(saveFilePath + Guid.NewGuid().ToString() + file.Name);
                    if (!Directory.Exists(saveFilePath))
                    {
                        Directory.CreateDirectory(saveFilePath);
                    }
                    var filepath = Path.Combine(filePath);

                    using var fileSteam = new FileStream(filepath, FileMode.Create);
                    await file.CopyToAsync(fileSteam);
                    result.Add(filePath);
                }
                return result;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput Add(CreateTicketViewModel model)
        {
            try
            {
                Add(new Ticket()
                {
                    Description = model.Description,
                    TicketStatus = model.TicketStatus,
                    SubmitterTeam = model.SubmitterTeam,
                    SubmitterEmail = model.SubmitterEmail,
                    ReportedSource = model.SubmitterEmail,
                    ServiceName = model.Team,
                    SubmitterName = model.SubmitterName,
                    TicketName = model.Summary,
                    SubmitDate = DateTime.Now,
                    TicketType = model.Type,
                    Source=model.Source,
                    Priority=model.Priority,
                    TicketSeverity = model.TicketSeverity,
                    ProductCategoryName1 = model.ProductCategoryName1,
                    ProductCategoryName2 = model.ProductCategoryName2,
                    TicketHistories = new List<TicketHistory> {
                        new TicketHistory
                        {
                            OldStatus = null,
                            NewStatus = Status.Created,
                            ModifiedDate = DateTime.Now,
                        }
                    },
                    LastModifiedDate = DateTime.Now,
                    Attachement = model.Attachement
                });

                //TODO: Handle SLA.
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        #region Private Methods

        private void PagingList(TicketPageInputViewModel model, TicketPageResultViewModel result, IEnumerable<Ticket> qurey)
        {
            result.TotalCount = qurey.Count();
            if (!model.IsPrint)
                qurey = qurey.Skip(model.PageNumber * model.PageSize).Take(model.PageSize);

            foreach (var item in qurey)
            {
                result.PageData.Add(new TicketPageData
                {
                    CanAssign = true,
                    CanView = true,
                    CanDelete = true,
                    CreationDate = item.SubmitDate,
                    Severity = item.TicketSeverity,
                    TicketTopic = item.ServiceName,
                    TicketType = item.TicketType,
                    RasiedBy = new RasiedByViewModel
                    {
                        Email = item.SubmitterEmail,
                        Name = item.SubmitterName
                    }
                });
            }
        }

        private IEnumerable<Ticket> SortList(TicketPageInputViewModel model, IEnumerable<Ticket> query)
        {
            switch (model.SortValue)
            {
                case TicketPageInputSort.RasiedBy:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SubmitterName);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SubmitterName);
                            break;
                        default:
                            break;
                    }
                    break;
                case TicketPageInputSort.CreationDate:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SubmitDate);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SubmitDate);
                            break;
                        default:
                            break;
                    }
                    break;
                case TicketPageInputSort.Topic:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.TicketName);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.TicketName);
                            break;
                        default:
                            break;
                    }
                    break;
                case TicketPageInputSort.Severity:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.TicketSeverity);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.TicketSeverity);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SubmitDate);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SubmitDate);
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return query;
        }

        private IEnumerable<Ticket> FilterList(TicketPageInputViewModel model, IEnumerable<Ticket> query)
        {
            if (!string.IsNullOrEmpty(model.SearchText))
                query = query.Where(e => e.TicketName.Contains(model.SearchText));
            return query;
        }

        private IEnumerable<Ticket> FilterLoggedUser(object userClaims, IEnumerable<Ticket> qurey, bool v)
        {
            return qurey;
        }

        public int GetCount()
        {
            return Count();
        }



        #endregion
    }
}
