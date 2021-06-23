using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.Incident;
using Halwani.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketController(ITicketRepository TicketRepository, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
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

        [HttpGet]
        [Route("getTicketNumbers")]
        public IActionResult getTicketNumbers()
        {
            var result = _TicketRepository.getTicketNO();
            if (result == null)
                return Problem();

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateTicket")]

        public IActionResult Post()
        {
            IEnumerable<IFormFile> attachements = null;
            if (Request.Form.Files != null && Request.Form.Files.Count() > 0)
                attachements = Request.Form.Files.ToList();

            var model = JsonConvert.DeserializeObject<CreateTicketViewModel>(_httpContextAccessor.HttpContext.Request.Form["data"]);
            TryValidateModel(model);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _TicketRepository.Add(model, attachements, _env.ContentRootPath + @"/files", User.FindFirstValue(ClaimTypes.NameIdentifier), HeadersHelper.GetAuthToken(Request));
            if (result == null || !result.Success)
                return Problem("");

            return Ok(result);

        }

        [HttpPut]
        [Route("UpdateTic")]
        public IActionResult Put()
        {
            IEnumerable<IFormFile> attachements = null;
            if (Request.Form.Files != null && Request.Form.Files.Count() > 0)
                attachements = Request.Form.Files.ToList();

            var model = JsonConvert.DeserializeObject<UpdateTicketModel>(_httpContextAccessor.HttpContext.Request.Form["data"]);
            TryValidateModel(model);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filePath = _env.ContentRootPath + @"/files/" + model.Id;
            var res = _TicketRepository.RemoveAttachments(filePath, model.Attachement.Split(','));
            model.Attachement = string.Join(",", res);
            var result = _TicketRepository.UpdateTicket(model, attachements, _env.ContentRootPath + @"/files", User.FindFirstValue(ClaimTypes.NameIdentifier), HeadersHelper.GetAuthToken(Request));
            if (result == null || !result.Success)
                return Problem("");
            return Ok(result);

        }

        //[HttpPost]
        //[Route("Create")]
        //public ActionResult CreateTicket(CreateTicketViewModel model)
        //{
        //    var result = _TicketRepository.Add(model, null, "");
        //    if (result.Code == RepositoryResponseStatus.Error)
        //        return Problem();
        //    return Ok();
        //}

        [HttpPost]
        [Route("UpdateStatus")]
        public ActionResult UpdateStatus(UpdateStatusViewModel model)
        {
            var result = _TicketRepository.UpdateStatus(model, User.FindFirstValue(ClaimTypes.NameIdentifier), HeadersHelper.GetAuthToken(Request));

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
            var result = _TicketRepository.UpdateStatus(model, User.FindFirstValue(ClaimTypes.NameIdentifier), HeadersHelper.GetAuthToken(Request));

            if (result.Code == RepositoryResponseStatus.Error)
                return Problem();

            if (result.Code == RepositoryResponseStatus.NotFound)
                return NotFound();

            return Ok();
        }

        //[HttpPut]
        //[Route("UpdateTicket/{id:long}")]
        //public ActionResult UpdateTicket(long Id, UpdateTicketModel model)
        //{
        //    var result = _TicketRepository.UpdateTicket(Id, model);

        //    if (result.Code == RepositoryResponseStatus.Error)
        //        return Problem();

        //    if (result.Code == RepositoryResponseStatus.NotFound)
        //        return NotFound();

        //    return Ok();
        //}

        [HttpPost]
        [Route("AssignTicket")]
        public ActionResult AssignTicket(AssignTicketViewModel model)
        {
            var result = _TicketRepository.AssignTicket(model,User.FindFirstValue(ClaimTypes.NameIdentifier),HeadersHelper.GetAuthToken(Request));

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
            var result = _TicketRepository.AssignTicket(model, User.FindFirstValue(ClaimTypes.NameIdentifier), HeadersHelper.GetAuthToken(Request));

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
            try
            {
                IFormFileCollection attachments = HttpContext.Request.Form.Files;
                string path = _env.ContentRootPath + @"/files";
                var result = _TicketRepository.PostFilesAsync(attachments, path).Result;
                if (result == null)
                    return Problem();
                return Ok(result);
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return Problem(ex.Message);
            }
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
            string path = configuration["Url:BaseServiceUrl"] + @"/files/";
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
