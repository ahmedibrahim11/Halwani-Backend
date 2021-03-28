using Halwani.Core.ModelRepositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            var result = _userRepositry.ListReporters();
            if (result == null)
                return Problem();
            return Ok(result);
        }

    }
}
