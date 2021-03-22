using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.RequestTypeModels;
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
    public class RequestTypeController : ControllerBase
    {

        private IRequestTypeRepository _requestTypeRepositry;

        public RequestTypeController(IRequestTypeRepository RequestTypeRepositry)
        {
            _requestTypeRepositry = RequestTypeRepositry;
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
    }
}
