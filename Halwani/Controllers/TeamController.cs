using Halwani.Core.ModelRepositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private ITeamRepository _teamRepositry;

        public TeamController(ITeamRepository TeamRepositry)
        {
            _teamRepositry = TeamRepositry;
        }

        [HttpGet]
        [Route("get")]
        public ActionResult Get()
        {
            var result = _teamRepositry.List();
            if (result == null)
                return Problem();
            return Ok(result);
        }

    }
}
