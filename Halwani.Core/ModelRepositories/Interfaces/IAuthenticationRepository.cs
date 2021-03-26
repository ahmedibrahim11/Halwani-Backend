using Halwani.Core.ViewModels.Authentication;
using Halwani.Core.ViewModels.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        LoginResponseViewModel Login(LoginModel model);
        UserSessionDataViewModel LoadUserSession(ClaimsIdentity userClaims);
    }
}
