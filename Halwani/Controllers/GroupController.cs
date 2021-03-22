using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GroupModels;
using Halwani.Data.Entities.ProductCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public GroupController(IGroupRepository GroupRepositry)
        {
            _groupRepositry = GroupRepositry;
        }

        [HttpGet]
        [Route("getGroup")]
        public ActionResult GetAllGroup()
        {
            var result = _groupRepositry.List();
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
    }
}
