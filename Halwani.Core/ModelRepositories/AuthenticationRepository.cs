using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.Authentication;
using Halwani.Core.ViewModels.AuthenticationModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Halwani.Core.ModelRepositories
{
    public class AuthenticationRepository : BaseRepository<User>, IAuthenticationRepository
    {
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseViewModel Login(LoginModel model)
        {
            try
            {
                //TODO: Replace with AD Authentication.
                var user = Find(e => e.UserName == model.UserName, null, "Role,UserTeams,UserTeams.Team").FirstOrDefault();
                if (user != null /*&& await userManager.CheckPasswordAsync(user, model.Password)*/)
                {
                    if (user.UserStatus != (int)UserStatusEnum.Active)
                        return new LoginResponseViewModel
                        {
                            Result = RepositoryOutput.CreateNotAllowedResponse()
                        };

                    GenerateJWTToken(user, out List<string> permissions, out JwtSecurityToken token);

                    return new LoginResponseViewModel
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        SetTicketHigh = user.SetTicketHigh,
                        Expiration = token.ValidTo,
                        Permissions = permissions,
                        UserProfile = new UserProfileModel
                        {
                            Id = user.Id,
                            Name = user.Name,
                            UserEmail = user.Email,
                            RoleEnum = (RoleEnum)user.RoleId,
                            UserName = user.UserName
                        },
                        Result = new RepositoryOutput()
                    };
                }
                return new LoginResponseViewModel
                {
                    Result = RepositoryOutput.CreateNotAllowedResponse()
                };
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return new LoginResponseViewModel
                {
                    Result = RepositoryOutput.CreateErrorResponse(ex.Message)
                };
            }
        }

        public UserSessionDataViewModel LoadUserSession(ClaimsIdentity userClaims)
        {
            try
            {
                var permissions = userClaims.Claims.FirstOrDefault(e => e.Type == AdditionalClaims.Permissions).Value;
                var teams = userClaims.Claims.FirstOrDefault(e => e.Type == AdditionalClaims.Teams).Value;
                //var isAllTeams = userClaims.Claims.FirstOrDefault(e => e.Type == AdditionalClaims.AllTeams).Value;

                return new UserSessionDataViewModel
                {
                    Id = long.Parse(userClaims.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier).Value),
                    Email = userClaims.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value,
                    Role = (RoleEnum)Enum.Parse(typeof(RoleEnum), userClaims.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Role).Value),
                    Name = userClaims.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Name).Value,
                    UserName = userClaims.Claims.FirstOrDefault(e => e.Type == ClaimTypes.UserData).Value,
                    Permissions = permissions?.Split(",").ToList(),
                    TeamsIds = teams?.Split(",").ToList(),
                    IsAllTeams = false
                };
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        #region Private

        private void GenerateJWTToken(User user, out List<string> permissions, out JwtSecurityToken token)
        {
            permissions = user.Role.Permissions?.Split(",").ToList();
            List<Claim> authClaims = FillUserClaims(user);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1).AddHours(Strings.AddedHoursToDate),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
        }

        private List<Claim> FillUserClaims(User user)
        {
            var teamPermission = user.UserTeams.Where(e => e.UserId == user.Id);
            var teamClaim = string.Join(",", teamPermission.Select(e => e.Team.Name));
            var authClaims =
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.UserData, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.RoleName),
                    new Claim(AdditionalClaims.Permissions, user.Role.Permissions == null ? "" : user.Role.Permissions),
                    new Claim(AdditionalClaims.Teams, teamClaim),
                    //new Claim(AdditionalClaims.AllTeams, teamPermission == null ? "false" : teamPermission.IsAllTeams.ToString())
                };

            return authClaims;
        }

        #endregion
    }
}
