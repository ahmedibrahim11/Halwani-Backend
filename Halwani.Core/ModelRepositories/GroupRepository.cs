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

namespace Halwani.Core.ModelRepositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public IEnumerable<GroupListViewModel> List()
        {
            try
            {
                return Find(null,null, "RequestTypeGroups,RequestTypeGroups.RequestType").Select(e => new GroupListViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    RequestTypes = e.RequestTypeGroups.Select(e=> new RequestTypeViewModel
                    {
                        Id = e.RequestType.Id,
                        Text = e.RequestType.Name,
                        TicketType = e.RequestType.TicketType,
                        Icon = e.RequestType.Icon
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
    }
}
