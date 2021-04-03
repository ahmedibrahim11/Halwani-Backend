using Halwani.Core.ModelRepositories;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.Incident;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
        [Authorize]
        [Route("List")]
        public ActionResult<TicketPageResultViewModel> List(TicketPageInputViewModel model)
        {
            var result = _TicketRepository.List(model, User.Identity as ClaimsIdentity, out RepositoryOutput response);
            if (response.Code == RepositoryResponseStatus.Error)
                return Problem(response.ErrorMessages.FirstOrDefault());

            return Ok(result);
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult CreateTicket(CreateTicketViewModel model)
            {
            var result = _TicketRepository.Add(model);
            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();
            return Ok();
        }

        [HttpPost]
        [Route("UpdateStatus")]
        public ActionResult UpdateStatus(UpdateStatusViewModel model)
        {
            var result = _TicketRepository.UpdateStatus(model);

            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();

            if (result.Code == RepositoryResponseStatus.NotFound)
                return NotFound();

            return Ok();
        }



        [HttpPut]
        [Route("UpdateTicket")]
        public ActionResult UpdateTicket(UpdateStatusViewModel model)
        {
            var result = _TicketRepository.UpdateStatus(model);

            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();

            if (result.Code == RepositoryResponseStatus.NotFound)
                return NotFound();

            return Ok();
        }



        [HttpPut]
        [Route("UpdateTicket/{id:long}")]
        public ActionResult UpdateTicket(long Id,UpdateTicketModel model)
        {
            var result = _TicketRepository.UpdateTicket(Id,model);

            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();

            if (result.Code == RepositoryResponseStatus.NotFound)
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Route("AssignTicket")]
        public ActionResult AssignTicket(AssignTicketViewModel model)
        {
            var result = _TicketRepository.AssignTicket(model);

            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();

            if (result.Code == RepositoryResponseStatus.NotFound)
                return NotFound();

            return Ok();
        }


        [HttpPost]
        [Route("AssignTickets")]
        public ActionResult AssignTicket(AssignMulipleTicketViewModel model)
        {
            var result = _TicketRepository.AssignTicket(model);

            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();

            if (result.Code == RepositoryResponseStatus.NotFound)
                return NotFound();

            return Ok();
        }

        [HttpPost]
        [Route("PostFile")]
        public IActionResult PostFile()
        {
            IFormFileCollection attachments = HttpContext.Request.Form.Files;
            string path = @"wwwroot\files\";
            var result = _TicketRepository.PostFilesAsync(attachments, path).Result;
            if (result == null)
                return Problem();
            return Ok(result);
        }
        [HttpGet]
        [Route("getCount")]
        public ActionResult GetCount()
        {
            var result = _TicketRepository.GetCount();
            if (result == null)
                return Problem();
            return Ok(result);
        }
        [HttpPost]
        [Route("getTicket")]
        public ActionResult getbyID([FromBody] IdDTO idObject)
        {
            string path = @"wwwroot\files\";
            var result = _TicketRepository.GetTicket(long.Parse(idObject.id), path);
            if (result == null)
                return Problem();
            return Ok(result);
        }

    }
    public class IdDTO
    {
        public string id { get; set; }
    }
}
