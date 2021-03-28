using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.TeamModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.Team;

namespace Halwani.Core.ModelRepositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public IEnumerable<LookupViewModel> List()
        {
            try
            {
                return Find().Select(e => new LookupViewModel
                {
                    Id = e.Id,
                    Text = e.Name
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        //public RepositoryOutput Add(List<CreateTeamModel> model)
        //{
        //    try
        //    {
        //        AddRange(model.Select(item => new Team()
        //        {
        //            Name = item.Name
        //        }).ToList());

        //        if (Save() < 1)
        //            return RepositoryOutput.CreateErrorResponse("");
        //        return RepositoryOutput.CreateSuccessResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        RepositoryHelper.LogException(ex);
        //        return RepositoryOutput.CreateErrorResponse(ex.Message);
        //    }
        //}
    }
}
