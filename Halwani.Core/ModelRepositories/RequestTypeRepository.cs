using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.RequestTypeModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halwani.Data.Entities.Incident;
using Microsoft.AspNetCore.Http;
using System.Transactions;
using System.IO;
using System.Security.Claims;
using Halwani.Data.Entities.User;
using Halwani.Core.ViewModels.TicketModels;

namespace Halwani.Core.ModelRepositories
{
    public class RequestTypeRepository : BaseRepository<RequestType>, IRequestTypeRepository
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public RequestTypeRepository(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public RequestTypeResultViewModel List(RequestTypeInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response)
        {
            try
            {
                response = new RepositoryOutput();
                RequestTypeResultViewModel result = new RequestTypeResultViewModel();


                var qurey = Find(null, null, "");

                qurey = FilterLoggedUser(userClaims, qurey);
                qurey = FilterList(model, userClaims, qurey);
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
        private IEnumerable<RequestType> FilterLoggedUser(ClaimsIdentity userClaims, IEnumerable<RequestType> query)
        {
            var userSession = _authenticationRepository.LoadUserSession(userClaims);
            if (userSession.Role != RoleEnum.User)
            {
                return query;
            }
            return new List<RequestType>();
        }
        private IEnumerable<RequestType> FilterList(RequestTypeInputViewModel model, ClaimsIdentity userClaims, IEnumerable<RequestType> query)
        {
            if (!string.IsNullOrEmpty(model.Filter.SearchText))
                query = query.Where(e => e.Name.ToLower().Contains(model.Filter.SearchText.ToLower())).ToList();
            if (model.Filter != null)
            {
                if (model.Filter.Priority.HasValue)
                    query = query.Where(e => e.Priority == model.Filter.Priority);
                if (model.Filter.Severity.HasValue)
                    query = query.Where(e => e.Severity == model.Filter.Severity);
                if (model.Filter.TicketType.HasValue)
                {
                    if (model.Filter.TicketType != TicketType.All)
                    {
                        query = query.Where(e => e.TicketType == model.Filter.TicketType);
                    }

                }

                if (model.Filter.GroupID.HasValue)
                {
                    query = query.Where(e => e.RequestTypeGroups.Any(t => t.GroupId == model.Filter.GroupID));
                }
                if (!string.IsNullOrEmpty(model.Filter.Team))
                {
                    query = query.Where(e => e.TeamName == model.Filter.Team);
                }
            }
            return query;
        }
        private IEnumerable<RequestType> SortList(RequestTypeInputViewModel model, IEnumerable<RequestType> query)
        {
            switch (model.SortValue)
            {
                case RequestTypeInputSort.Title:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.Name);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.Name);
                            break;
                        default:
                            break;
                    }
                    break;
                case RequestTypeInputSort.Team:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.TeamName);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.TeamName);
                            break;
                        default:
                            break;
                    }
                    break;
                case RequestTypeInputSort.TicketType:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.TicketType);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.TicketType);
                            break;
                        default:
                            break;
                    }
                    break;
                case RequestTypeInputSort.Group:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.RequestTypeGroups.Count);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.RequestTypeGroups.Count);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.Name);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.Name);
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return query;
        }
        private void PagingList(RequestTypeInputViewModel model, RequestTypeResultViewModel result, IEnumerable<RequestType> qurey)
        {
            result.TotalCount = qurey.Count();
            if (!model.IsPrint)
                qurey = qurey.Skip(model.PageNumber * model.PageSize).Take(model.PageSize);

            foreach (var item in qurey)
            {
                result.PageData.Add(new RequestTypeData
                {
                    ID = item.Id,
                    Title = item.Name,
                    Description = item.Description,
                    Group = item.RequestTypeGroups.Count > 1 ? "(Used in 2 groups)" : item.RequestTypeGroups.First().Group.Name,
                    Team = item.TeamName,
                    TicketType = (int)item.TicketType,
                    IsVisable = item.IsVisible



                });
            }
        }


        public IEnumerable<RequestTypeListViewModel> List()
        {
            try
            {
                var groupBy = Find(null, null, "DefaultTeam").GroupBy(e => e.TicketType);

                return groupBy.Select(e => new RequestTypeListViewModel
                {
                    TicketType = e.FirstOrDefault().TicketType,
                    IsVisible = e.FirstOrDefault().IsVisible,
                    Topics = e.Select(a => new RequestTypeViewModel
                    {
                        Id = a.Id,
                        Text = a.Name,
                        TicketType = a.TicketType,
                        Icon = a.Icon,
                        Description = a.Description,
                        TeamName = a.TeamName,
                        Priority = a.Priority,
                        Severity = a.Severity
                    })
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput Add(List<CreateRequestTypeModel> model)
        {
            try
            {
                AddRange(model.Select(item => new RequestType()
                {
                    Name = item.Name,
                    Icon = item.Icon,
                    TicketType = item.TicketType,
                    Description = item.Description,
                    TeamName = item.TeamName,
                    Priority = item.Priority,
                    Severity = item.Severity,
                    RequestTypeGroups = item.GroupIds.Select(e => new RequestTypeGroups
                    {
                        GroupId = e
                    }).ToList()
                }).ToList());

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

        public RepositoryOutput Add(CreateRequestTypeModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token)
        {
            try
            {
                var RT = new RequestType()
                {
                    Name = model.Name,
                    Icon = model.Icon == null ? "" : model.Icon,
                    TicketType = model.TicketType,
                    Description = model.Description,
                    TeamName = model.TeamName,
                    Priority = model.Priority,
                    Severity = model.Severity,
                    RequestTypeGroups = model.GroupIds.Select(e => new RequestTypeGroups
                    {
                        GroupId = e
                    }).ToList()
                };
                Add(RT);
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");
                    List<string> result = StoreFiles(attachments, null, saveFilePath, RT);

                    RT.Icon = string.Join(",", result);
                    Update(RT);
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");

                    scope.Complete();
                }

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public RepositoryOutput Update(CreateRequestTypeModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token)
        {
            try
            {
                var RT = Find(e => e.Id == model.Id.Value).FirstOrDefault();
                RT.Name = model.Name;
                RT.Icon = model.Icon == null ? "" : model.Icon;
                RT.TicketType = model.TicketType;
                RT.Description = model.Description;
                RT.TeamName = model.TeamName;
                RT.Priority = model.Priority;
                RT.Severity = model.Severity;
                RT.RequestTypeGroups = model.GroupIds.Select(e => new RequestTypeGroups
                {
                    GroupId = e
                }).ToList();

                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");
                    if (attachments != null)
                    {
                        List<string> result = StoreFiles(attachments, null, saveFilePath, RT);

                        RT.Icon = string.Join(",", result);
                    }

                    Update(RT);
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");

                    scope.Complete();
                }

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public RepositoryOutput UpdateVisiblity(int id, bool isVisible)
        {
            try
            {
                var RT = Find(e => e.Id == id).FirstOrDefault();
                RT.IsVisible = isVisible;
                Update(RT);
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

        private static List<string> StoreFiles(IEnumerable<IFormFile> attachments, List<string> oldAttachments, string saveFilePath, RequestType RT)
        {
            var result = new List<string>();
            if (attachments == null)
                return result;

            foreach (var file in attachments)
            {
                var fileName = Guid.NewGuid().ToString() + file.FileName;
                var filePath = saveFilePath + "/" + RT.Id + "/" + fileName;
                if (!Directory.Exists(saveFilePath + "/" + RT.Id))
                {
                    Directory.CreateDirectory(saveFilePath + "/" + RT.Id);
                }
                using var fileSteam = new FileStream(filePath, FileMode.Create);
                file.CopyToAsync(fileSteam).GetAwaiter().GetResult();
                result.Add(fileName);
            }
            if (oldAttachments != null)
            {
                foreach (var item in oldAttachments)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public GetRequestType Get(int id, string rootFile)
        {
            var query = Find(r => r.Id == id).FirstOrDefault();
            var RT = new GetRequestType();
            if (query != null)
            {
                RT = new GetRequestType()
                {
                    ID = query.Id,
                    Title = query.Name,
                    Priority = query.Priority,
                    Sevirity = query.Severity,
                    Description = query.Description,
                    TicketType = (int)query.TicketType,
                    Team = query.TeamName,
                    Icon = rootFile+ "/" + RT.ID + "/" + query.Icon,

                };
            }

            return RT;
        }
    }
}
