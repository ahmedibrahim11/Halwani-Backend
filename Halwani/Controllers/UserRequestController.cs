using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.RequestTypeModels;
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
    public class UserRequestController : ControllerBase
    {

        private IUserRequestRepository _userRequestRepositry;

        public UserRequestController(IUserRequestRepository categoryRepositry)
        {
            _userRequestRepositry = categoryRepositry;
        }

        [HttpPut]
        [Route("AskForSupport")]
        public ActionResult AskForSupport(UserRequestViewModel model)
        {
            var result = _userRequestRepositry.AskForSupport(model, User.Identity as ClaimsIdentity);
            if (result.Code == RepositoryResponseStatus.Error)
                return Problem(); 
            return Ok();
        }

        [HttpPut]
        [Route("ReportBug")]
        public ActionResult ReportBug(UserRequestViewModel model)
        {
            var result = _userRequestRepositry.ReportBug(model, User.Identity as ClaimsIdentity);
            if (result.Code == RepositoryResponseStatus.Error)
                return Problem(); 
            return Ok();
        }
    }
}
