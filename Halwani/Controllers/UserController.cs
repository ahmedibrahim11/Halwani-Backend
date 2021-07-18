using Halwani.Core.ModelRepositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private IUserRepository _userRepositry;

        public UserController(IUserRepository UserRepositry)
        {
            _userRepositry = UserRepositry;
        }

        [HttpGet]
        [Route("getUser")]
        public ActionResult GetAllUser()
        {
            var result = _userRepositry.ListReporters(User.Identity as ClaimsIdentity);
            if (result == null)
                return Problem();
            return Ok(result);
        }


        [HttpGet]
        [Route("getItPersonal/{teamId:long}")]
        public ActionResult GetItPersonal(string teamName)
        {
            var result = _userRepositry.ListItPersonal(teamName);
            if (result == null)
                return Problem();
            return Ok(result);
        }

    }
}
