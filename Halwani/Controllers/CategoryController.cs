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
            return Ok();
        }


        [HttpPost]
        [Route("create")]
        public ActionResult CreateCategoryProduct(List<CreateProductCategroyModel> model)
        {

            foreach (var item in model)
            {
                ProductCategory productCategory = new ProductCategory()
                {
                    Name = item.ParentCategory,
                };

                _categoryRepositry.Add(productCategory);
                _categoryRepositry.Save();
                foreach (var subcategory in item.SubCategory)
                {
                    ProductCategory subCategory = new ProductCategory()
                    {
                        Name = subcategory.SubCategoryName,
                        Parent=productCategory,
                        ParentCategoryId = productCategory.Id
                    };
                    _categoryRepositry.Add(subCategory);
                    _categoryRepositry.Save();
                }


            }




            return Ok();
        }


    }
}
