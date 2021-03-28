using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Halwani.Data.Entities.User;
using Halwani.Core.ModelRepositories.Interfaces;

namespace Halwani.Core.ModelRepositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public IEnumerable<LookupViewModel> ListReporters()
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

        //public RepositoryOutput Add(List<CreateUserModel> model)
        //{
        //    try
        //    {
        //        AddRange(model.Select(item => new User()
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
