using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GroupModels;
using Halwani.Data.Entities.ProductCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private IGroupRepository _groupRepositry;
        private readonly IConfiguration configuration;

        public GroupController(IGroupRepository GroupRepositry, IConfiguration _configuration)
        {
            configuration = _configuration;
            _groupRepositry = GroupRepositry;
        }

        [HttpGet]
        [Route("getGroup")]
        public ActionResult GetAllGroup()
        {
            string path = configuration["Url:BaseServiceUrl"] + @"/files/";
            var result = _groupRepositry.List(path);
            if (result == null)
                return Problem();
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult CreateGroupProduct(List<CreateGroupModel> model)
        {
            var result = _groupRepositry.Add(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            return Ok();
        }
        [HttpPost]
        [Route("createOne")]
        public ActionResult CreateOne(CreateGroupModel model)
        {
            var result = _groupRepositry.AddOne(model);
            if (result==0)
                return Problem();
            return Ok(result);
        }
        [HttpPost]
        [Route("getGroupforTicketType")]
        public ActionResult getGroupforTicketType([FromBody] myID ticketType)
        {
            var result = _groupRepositry.listTicketTypeGroups(ticketType.id);
            if (result == null)
                return Problem();
            return Ok(result);
        }
        [HttpPost]
        [Route("getGroupforTicketTypeEdit")]
        public ActionResult getGroupforTicketTypeEdit([FromBody] myID ticketType)
        {
            var result = _groupRepositry.listTicketTypeGroups(ticketType.id,ticketType.rtid);
            if (result == null)
                return Problem();
            return Ok(result);
        }
        public class myID
        {
            public int id { get; set; }
            public int rtid { get; set; }
        }
    }
}
