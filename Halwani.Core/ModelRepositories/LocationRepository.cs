using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halwani.Core.ModelRepositories
{
    public class LocationRepository : BaseRepository<Location>, IlocationRepository
    {
        public IEnumerable<LookupViewModel> GetLocations()
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
    }
}
