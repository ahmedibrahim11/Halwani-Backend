using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Team;
using System.Collections.Generic;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        IEnumerable<LookupViewModel> List();
    }
}
