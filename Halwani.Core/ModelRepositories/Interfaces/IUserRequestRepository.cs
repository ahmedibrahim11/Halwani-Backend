using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.RequestTypeModels;
using System.Security.Claims;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IUserRequestRepository
    {
        RepositoryOutput AskForSupport(UserRequestViewModel model, ClaimsIdentity userClaims);
        RepositoryOutput ReportBug(UserRequestViewModel model, ClaimsIdentity userClaims);
    }
}
