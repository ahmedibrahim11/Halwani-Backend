using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.CategoryModels;
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
    public class CategoryController : ControllerBase
    {

        private ICategoryRepository _categoryRepositry;

        public CategoryController(ICategoryRepository categoryRepositry)
        {
            _categoryRepositry = categoryRepositry;
        }

        [HttpGet]
        [Route("getCategory")]
        public ActionResult GetAllCategory()
        {
            var result = _categoryRepositry.List();
            if (result == null)
                return Problem();
            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult CreateCategoryProduct(List<CreateProductCategroyModel> model)
        {
            var result = _categoryRepositry.Add(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem();
            return Ok();
        }
    }
}
