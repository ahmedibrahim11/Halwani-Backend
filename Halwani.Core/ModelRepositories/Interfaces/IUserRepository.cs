using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.UserModels;
using Halwani.Data.Entities.User;
using System.Collections.Generic;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        IEnumerable<UserLookupViewModel> ListReporters();
    }
}
