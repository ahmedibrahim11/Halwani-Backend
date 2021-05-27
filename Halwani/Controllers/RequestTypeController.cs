using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.RequestTypeModels;
using Halwani.Data.Entities.ProductCategories;
using Halwani.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Halwani.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestTypeController : ControllerBase
    {

        private IRequestTypeRepository _requestTypeRepositry;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public RequestTypeController(IWebHostEnvironment env,IRequestTypeRepository RequestTypeRepositry, IHttpContextAccessor HttpContextAccessor)
        {
            _env = env;
            _requestTypeRepositry = RequestTypeRepositry;
            _httpContextAccessor = HttpContextAccessor;
        }

        [HttpGet]
        [Route("getRequestType")]
        public ActionResult GetAllRequestType()
        {
            var result = _requestTypeRepositry.List();
            if (result == null)
                return Problem();
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult CreateRequestTypeProduct(List<CreateRequestTypeModel> model)
        {

            var result = _requestTypeRepositry.Add(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            return Ok();
        }
        [HttpPost]
        [Route("CreateRequestType")]
        public IActionResult CreateRequestType()
        {
            IEnumerable<IFormFile> attachements = null;
            if (Request.Form.Files != null && Request.Form.Files.Count() > 0)
                attachements = Request.Form.Files.ToList();

            var model = JsonConvert.DeserializeObject<CreateRequestTypeModel>(_httpContextAccessor.HttpContext.Request.Form["data"]);
            TryValidateModel(model);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _requestTypeRepositry.Add(model, attachements, _env.ContentRootPath + @"/files", User.FindFirstValue(ClaimTypes.NameIdentifier), HeadersHelper.GetAuthToken(Request));
            if (result == null || !result.Success)
                return Problem("");

            return Ok(result);
        }

    }
}
