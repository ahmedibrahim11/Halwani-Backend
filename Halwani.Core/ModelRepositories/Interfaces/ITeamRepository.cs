using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Team;
using System.Collections.Generic;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Team GetByName(string name);
        IEnumerable<LookupViewModel> List();
    }
}
