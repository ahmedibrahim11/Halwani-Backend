using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halwani.Core.ModelRepositories
{
    public class CategoryRepository : BaseRepository<ProductCategory>, ICategoryRepository
    {
        public IEnumerable<CategoryListViewModel> List()
        {
            try
            {
                return Find(e=> !e.ParentCategoryId.HasValue, null, "ProductCategories").Select(e => new CategoryListViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    Children = e.ProductCategories.Select(c=> new ViewModels.GenericModels.LookupViewModel
                    {
                        Id = c.Id,
                        Text = c.Name
                    })
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput Add(List<CreateProductCategroyModel> model)
        {
            try
            {
                AddRange(model.Select(item=> new ProductCategory()
                {
                    Name = item.ParentCategory,
                    ProductCategories = item.SubCategory.Select(e => new ProductCategory
                    {
                        Name = e.SubCategoryName,
                    }).ToList()
                }).ToList());
                
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");
                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }
    }
}
