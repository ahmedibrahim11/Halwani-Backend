using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.SLAModels;
using Halwani.Data.Entities.ProductCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SLAController : ControllerBase
    {

        private ISLARepository _SLARepositry;

        public SLAController(ISLARepository SLARepositry)
        {
            _SLARepositry = SLARepositry;
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult Create(SLAModel model)
        {
            var result = _SLARepositry.Add(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            return Ok();
        }

        [HttpPut]
        [Route("Edit")]
        public ActionResult Edit(SLAModel model)
        {
            var result = _SLARepositry.Edit(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.NotFound)
                return NotFound();
            return Ok();
        }
        [HttpPost]
    
        [Route("List")]
        public ActionResult<SLAResultViewModel> List(SLAPageInputViewModel model)
        {
            var result = _SLARepositry.List(model, User.Identity as ClaimsIdentity, out RepositoryOutput response);
            if (response.Code == RepositoryResponseStatus.Error)
                return Problem(response.ErrorMessages.FirstOrDefault());

            return Ok(result);
        }
        [HttpGet]

        [Route("GetForEdit/{settingID:long}")]
        public ActionResult GetForEdit(long settingID)
        {
            var result = _SLARepositry.GetForEdit(settingID);
            if (result == null)
                return Problem();

            return Ok(result);
        }
    }
}
