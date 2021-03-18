using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.Message;
using Microsoft.AspNetCore.Mvc;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketMessageController : ControllerBase
    {
        private ITicketMessageRepository _messageRepositry;

        public TicketMessageController(ITicketMessageRepository messageRepositry)
        {
            _messageRepositry = messageRepositry;
        }

        [HttpPost]
        [Route("getMessages")]
        public ActionResult getbyID([FromBody] IdDTO idObject)
        {
            var result = _messageRepositry.List(long.Parse(idObject.id));
            if (result == null)
                return Problem();
            return Ok(result);
        }
        [HttpPost]
        [Route("create")]
        public ActionResult CreateCategoryProduct(TicketMessageDTO model)
        {
            var result = _messageRepositry.Add(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            return Ok();
        }
    }
    
}
