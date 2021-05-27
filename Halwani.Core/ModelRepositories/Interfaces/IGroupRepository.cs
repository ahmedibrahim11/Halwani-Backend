using Halawani.Core;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.GroupModels;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        IEnumerable<GroupListViewModel> List();
        RepositoryOutput Add(List<CreateGroupModel> model);
        IEnumerable<GroupList> listTicketTypeGroups(int ticketType);
        int AddOne(CreateGroupModel model);
    }
}
