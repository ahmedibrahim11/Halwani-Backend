using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GroupModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halwani.Data.Entities.Incident;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Halwani.Data.Entities.User;

namespace Halwani.Core.ModelRepositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public GroupRepository(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public IEnumerable<GroupListViewModel> List(string rootFile)
        {
            try
            {
                return Find(null, null, "RequestTypeGroups,RequestTypeGroups.RequestType,RequestTypeGroups.RequestType.DefaultTeam").Select(e => new GroupListViewModel
                {
                    Id = e.Id,
                    Text = e.Name, 
                    RequestTypes = e.RequestTypeGroups.Where(g => g.RequestType.IsVisible).Select(e => new RequestTypeViewModel
                    {
                        Id = e.RequestType.Id,
                        Text = e.RequestType.Name,
                        TicketType = e.RequestType.TicketType,
                        Icon = rootFile + "/" + e.RequestTypeId + "/" + e.RequestType.Icon,
                        Description = e.RequestType.Description,
                        DefaultTeam = e.RequestType.TeamName,
                        Priority=e.RequestType.Priority,
                        TicketSeverity=e.RequestType.Severity
                    })
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput Add(List<CreateGroupModel> model)
        {
            try
            {
                AddRange(model.Select(item => new Group()
                {
                    Name = item.Name,
                    Description=item.Description
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
        public int AddOne(CreateGroupModel model)
        {
            try
            {
                var addedGroup = Add(new Group() { Name = model.Name ,Description=model.Description});

                if (Save() < 1)
                    return 0;
                return addedGroup.Id;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return 0;
            }
        }

        public IEnumerable<GroupList> GetAllGroups()
        {
            try
            {
                return Find(null, null, null).Select(s => new GroupList
                {
                    ID = s.Id,
                    Name = s.Name,
                    Selected = false
                });

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<GroupList> listTicketTypeGroups()
        {
            try
            {

                var selected = Find(r=>r.IsVisible==true).
                     Select(r => new GroupList { ID = r.Id, Name = r.Name, Selected = false });

                return selected;

            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }
        public IEnumerable<GroupList> listTicketTypeGroups(int ticketType, int RTID)
        {
            try
            {
                var selected = Find(r => r.RequestTypeGroups.Any(r => r.RequestTypeId == RTID), null, "").
                     Select(r => new GroupList { ID = r.Id, Name = r.Name, Selected = true });
                var unselected = Find(r => r.RequestTypeGroups.Count == 0, null, "").
                    Select(r => new GroupList { ID = r.Id, Name = r.Name, Selected = false });
                return selected.Concat(unselected);

            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }
        public GroupResultViewModel List(GroupPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response)
        {
            try
            {
                response = new RepositoryOutput();
                GroupResultViewModel result = new GroupResultViewModel();


                var qurey = Find(null, null, "");

               
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
        private IEnumerable<Group> FilterLoggedUser(ClaimsIdentity userClaims, IEnumerable<Group> query)
        {
            var userSession = _authenticationRepository.LoadUserSession(userClaims);
            if (userSession.Role != RoleEnum.User)
            {
                return query;
            }
            return new List<Group>();
        }
        private IEnumerable<Group> FilterList(GroupPageInputViewModel model, ClaimsIdentity userClaims, IEnumerable<Group> query)
        {
            if(model.SearchText.Length>0)
            {
                query = query.Where(r => r.Name == model.SearchText[0]);
            }

            return query;
        }
        private IEnumerable<Group> SortList(GroupPageInputViewModel model, IEnumerable<Group> query)
        {
            switch (model.SortValue)
            {
                case GroupPageInputSort.Name:
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
                //case SLAPageInputSort.Team:
                //    switch (model.SortDirection)
                //    {
                //        case SortDirection.Asc:
                //            query = query.OrderBy(e => e.ServiceLine);
                //            break;
                //        case SortDirection.Des:
                //            query = query.OrderByDescending(e => e.ServiceLine);
                //            break;
                //        default:
                //            break;
                //    }
                //    break;
                case GroupPageInputSort.TopicCount:
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
               
            }
            return query;
        }
        private void PagingList(GroupPageInputViewModel model, GroupResultViewModel result, IEnumerable<Group> qurey)
        {
            result.TotalCount = qurey.Count();
            if (!model.IsPrint)
                qurey = qurey.Skip(model.PageNumber * model.PageSize).Take(model.PageSize);

            foreach (var item in qurey)
            {
                result.PageData.Add(new GroupReturnedModel
                {
                    ID = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    TopicCount=item.RequestTypeGroups.Count,
                    isVisable=item.IsVisible
                    
                  
                   
                });
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
        public CreateGroupModel GetForEdit(int ID)
        {
            var sla = Find(r => r.Id == ID).FirstOrDefault();
            return new CreateGroupModel()
            {
                ID = sla.Id,
                Name = sla.Name,
                Description = sla.Description,
               
            };
        }
        public RepositoryOutput Edit(CreateGroupModel model)
        {
            try
            {
                var old = Find(e => e.Id == model.ID).FirstOrDefault();
                if (old == null)
                    return RepositoryOutput.CreateNotFoundResponse();
                old.Name = model.Name;
                old.Description = model.Description;
                
               

                Update(old);
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse();
                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse();
            }
        }


    }
}
