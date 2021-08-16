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

namespace Halwani.Core.ModelRepositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
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
                    Name = item.Name
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
                var addedGroup = Add(new Group() { Name = model.Name });

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

        public IEnumerable<GroupList> listTicketTypeGroups(int ticketType)
        {
            try
            {

                var selected = Find(r => r.RequestTypeGroups.Any(l => l.RequestType.TicketType == (TicketType)ticketType), null, "").
                     Select(r => new GroupList { ID = r.Id, Name = r.Name, Selected = false });
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
    }
}
