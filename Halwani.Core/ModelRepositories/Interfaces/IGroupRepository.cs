using Halawani.Core;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.GroupModels;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        IEnumerable<GroupListViewModel> List(string rootFile);
        RepositoryOutput Add(List<CreateGroupModel> model);
        IEnumerable<GroupList> listTicketTypeGroups();
        IEnumerable<GroupList> listTicketTypeGroups(int ticketType, int RTID);
        int AddOne(CreateGroupModel model);
        GroupResultViewModel List(GroupPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response);
        RepositoryOutput UpdateVisiblity(int id, bool isVisible);
        CreateGroupModel GetForEdit(int ID);
        RepositoryOutput Edit(CreateGroupModel model);
    }
}
