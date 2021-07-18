using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Halwani.Data.Entities.User;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.UserModels;
using System.Security.Claims;

namespace Halwani.Core.ModelRepositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public IEnumerable<UserLookupViewModel> ListItPersonal(string teamName)
        {

            try
            {
                return Find(s => s.UserTeams.Any(t => t.Team.Name == teamName && t.User.RoleId == (int)RoleEnum.ItPersonal)).Select(e => new UserLookupViewModel
                {
                    Id = e.Id,
                    Text=e.Name,
                    UserName = e.UserName,
                    Email = e.Email
                }).ToList();

            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }

        }

        public IEnumerable<UserLookupViewModel> ListReporters(ClaimsIdentity claimsIdentity)
        {
            try
            {
                var teams = claimsIdentity.Claims.FirstOrDefault(e => e.Type == AdditionalClaims.Teams).Value;

                return Find(s => s.UserTeams.Any(ut => teams.Contains(ut.Team.Name)) && s.RoleId == (int)RoleEnum.User).Select(e => new UserLookupViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    Email = e.Email,
                    UserName = e.UserName,
                    Team = string.Join(",", e.UserTeams.Select(e => e.Team.Name))
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
