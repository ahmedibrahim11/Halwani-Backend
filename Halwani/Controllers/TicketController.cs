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
        //[ValidateToken]
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
            var result = _TicketRepository.GetById(long.Parse(idObject.id));
            if (result == null)
                return Problem();
            return Ok(result);
        }

    }
    public class IdDTO {
        public string id { get; set; }
    } }
