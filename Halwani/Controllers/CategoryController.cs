using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
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

        [HttpPut]
        [Route("edit")]
        public ActionResult EditCategoryProduct(CreateProductCategroyModel model)
        {
            var result = _categoryRepositry.Edit(model);
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.Error)
                return Problem(); ;
            if (result.Code == Core.ViewModels.GenericModels.RepositoryResponseStatus.NotFound)
                return NotFound();
            return Ok();
        }

        [HttpPost]
        [Route("List")]
        public ActionResult List(CategoryPageInputViewModel model)
        {
            var result = _categoryRepositry.List(model, out RepositoryOutput response);
            if (response.Code == RepositoryResponseStatus.Error)
                return Problem(response.ErrorMessages.FirstOrDefault());

            return Ok(result);
        }

        [HttpGet]
        [Route("GetForEdit/{settingID:long}")]
        public ActionResult GetForEdit(long settingID)
        {
            var result = _categoryRepositry.GetForEdit(settingID);
            if (result == null)
                return Problem();

            return Ok(result);
        }


        [HttpGet]
        [Route("UpdateVisiblity")]
        public ActionResult UpdateVisibility(int id, bool isVisible)
        {
            var result = _categoryRepositry.UpdateVisiblity(id, isVisible);
            if (!result.Success)
                return Problem("");
            return Ok(result);
        }
    }
}
