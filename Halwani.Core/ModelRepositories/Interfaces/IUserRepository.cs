using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.User;
using System.Collections.Generic;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        IEnumerable<LookupViewModel> ListReporters();
    }
}
