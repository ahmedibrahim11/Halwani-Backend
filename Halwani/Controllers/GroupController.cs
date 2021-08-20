using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.GroupModels;
using Halwani.Data.Entities.ProductCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Route("List")]
        public ActionResult<GroupResultViewModel> List(GroupPageInputViewModel model)
        {
            var result = _groupRepositry.List(model, User.Identity as ClaimsIdentity, out RepositoryOutput response);
            if (response.Code == RepositoryResponseStatus.Error)
                return Problem(response.ErrorMessages.FirstOrDefault());

            return Ok(result);
        }
        [HttpPost]
        [Route("getGroup")]
        public ActionResult getGroupforTicketType()
        {
            var result = _groupRepositry.listTicketTypeGroups();
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
        [HttpGet]
        [Authorize]
        [Route("UpdateVisiblity")]
        public ActionResult UpdateVisibility(int id, bool isVisible)
        {
            var result = _groupRepositry.UpdateVisiblity(id, isVisible);
            if (!result.Success)
                return Problem("");
            return Ok(result);
        }
        [HttpGet]

        [Route("GetForEdit/{settingID:int}")]
        public ActionResult GetForEdit(int settingID)
        {
            var result = _groupRepositry.GetForEdit(settingID);
            if (result == null)
                return Problem();

            return Ok(result);
        }
        [HttpPut]
        [Route("Edit")]
        public ActionResult Edit(CreateGroupModel model)
        {
            var result = _groupRepositry.Edit(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.NotFound)
                return NotFound();
            return Ok();
        }
        public class myID
        {
            public int id { get; set; }
            public int rtid { get; set; }
        }
    }
}
