using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationRepository _authenticateRepository;

        public AuthenticationController(IAuthenticationRepository authenticateRepository)
        {
            _authenticateRepository = authenticateRepository;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel model)
        {
            var result = _authenticateRepository.Login(model);

            if (result.Result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.NotAllowed)
                return Forbid();
            if (result.Result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem(result.Result.ErrorMessages.FirstOrDefault());

            return Ok(result);
        }
    }
}
