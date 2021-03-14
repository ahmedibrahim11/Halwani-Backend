using Halwani.Core.ModelRepositories;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.Incident;
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
        //[ValidateToken]
        [Route("Create")]
        public ActionResult CreateTicket(CreateTicketViewModel model)
        {
            Ticket newTikcet = new Ticket()
            {
                Description = model.Description,
                TicketStatus = model.TicketStatus,
                SubmitterTeam = model.SubmitterTeam,
                SubmitterEmail = model.SubmitterEmail,
                ReportedSource = model.SubmitterEmail,
                ServiceName = model.ServiceName,
                SubmitterName = model.SubmitterName,
                TicketName = model.Summary,
                SubmitDate = DateTime.Now,
                TicketType = model.Type,
                TicketSeverity = model.TicketSeverity
            };
            var result = _TicketRepository.Add(newTikcet);
            result.TicketHistories.Add(new TicketHistory
            {
                FromTeam = result.SubmitterTeam,
                ToTeam = result.SubmitterTeam,
                OldStatus = result.TicketStatus,
                NewStatus = result.TicketStatus,
                ModifiedDate = DateTime.Now
            });
            _TicketRepository.Save();
            return Ok();
        }



        [HttpPost]
        [Route("PostFile")]
        public async Task<IActionResult> PostFile()
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                string path = @"wwwroot\files\";
                var file = files[0];
                var folderpath = Path.Combine(path + file.Name);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
                var filepath = Path.Combine(folderpath + "/" + file.FileName);

                using (var fileSteam = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSteam);
                    return Ok(new { filePath = file.Name });
                }
            }
            catch (Exception e)
            { 

                throw e;
            }

        }
    }
}
