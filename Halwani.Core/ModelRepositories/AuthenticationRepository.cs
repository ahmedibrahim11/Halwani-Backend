using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ViewModels.Authentication;
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
    public class AuthenticationRepository : BaseRepository<User>
    {
        public AuthenticationRepository()
        {

        }


        public LoginResponseViewModel Login(LoginModel model, IConfiguration _configuration, bool isEnglish)
        {
            try
            {
                var user = Find(e=> e.Email == model.Email).FirstOrDefault();
                if (user != null /*&& await userManager.CheckPasswordAsync(user, model.Password)*/)
                {
                    //if (user.UserStatusEnum != (int)UserStatus.Active || user.IsDeleted)
                    //    return new LoginResponseViewModel
                    //    {
                    //        Result = RepositoryOutput.CreateNotAllowedResponse()
                    //    };

                    GenerateJWTToken(_configuration, user, out List<string> permissions, out JwtSecurityToken token);

                    return new LoginResponseViewModel
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo,
                        Permissions = permissions,
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
                    Result = RepositoryOutput.CreateErrorResponse()
                };
            }
        }

        private void GenerateJWTToken(IConfiguration _configuration, User user, out List<string> permissions, out JwtSecurityToken token)
        {
            permissions = null;
            List<Claim> authClaims = FillUserClaims(user, out permissions);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
        }

        private List<Claim> FillUserClaims(User user, out List<string> permissions)
        {
            permissions = new List<string>();
            var authClaims =
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    //new Claim(CustomClaims.UserType.ToString(), user.UserTypeEnum.ToString()),
                    //new Claim(CustomClaims.LastUpdateDate.ToString(), user.UpdatedDate.HasValue ? user.UpdatedDate.Value.ToString():user.CreationDate.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role.RoleName)
                };

            //authClaims.AddRange(permissions.Select(e => new Claim(CustomClaims.Permissions.ToString(), e)));

            return authClaims;
        }

    }
}
