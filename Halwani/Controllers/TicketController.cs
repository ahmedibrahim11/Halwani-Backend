using Halwani.Core.ModelRepositories;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.TicketModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private ITicketRepository _TicketRepository;

        public TicketController(ITicketRepository TicketRepository)
        {
            _TicketRepository = TicketRepository;
        }

        [HttpPost]
        //[ValidateToken]
        [Route("List")]
        public ActionResult<TicketPageResultViewModel> List(TicketPageInputViewModel model)
        {
            var result = _TicketRepository.List(model, User.Identity as ClaimsIdentity, out RepositoryOutput response);
            if (response.Code == RepositoryResponseStatus.Error)
                return Problem(response.ErrorMessages.FirstOrDefault());

            return Ok(result);
        }

    }
}
