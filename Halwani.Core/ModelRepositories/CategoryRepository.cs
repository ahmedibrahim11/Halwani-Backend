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
                return Find(e => !e.ParentCategoryId.HasValue, null, "ProductCategories").Select(e => new CategoryListViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    Children = e.ProductCategories.Select(c => new LookupViewModel
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
                AddRange(model.Select(item => new ProductCategory()
                {
                    Name = item.ParentCategory,
                    ProductCategories = item.SubCategory.Select(e => new ProductCategory
                    {
                        Name = e.SubCategoryName,
                        Goal = e.Goal
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

        public RepositoryOutput Edit(CreateProductCategroyModel model)
        {
            try
            {
                var old = Find(e => e.Id == model.ParentCategoryId).FirstOrDefault();
                if (old == null)
                    return RepositoryOutput.CreateNotFoundResponse();

                old.Name = model.ParentCategory;
                foreach (var item in model.SubCategory.Where(e=> !e.SubCategoryId.HasValue))
                {
                    old.ProductCategories.Add(new ProductCategory
                    {
                        Name = item.SubCategoryName,
                        Goal = item.Goal
                    });
                }
                foreach (var item in model.SubCategory.Where(e=> e.SubCategoryId.HasValue && !e.IsDeleted))
                {
                    var oldSub = old.ProductCategories.FirstOrDefault(e => e.Id == item.SubCategoryId);
                    oldSub.Name = item.SubCategoryName;
                    oldSub.Goal = item.Goal;
                }
                foreach (var item in model.SubCategory.Where(e=> e.SubCategoryId.HasValue && e.IsDeleted))
                {
                    var oldSub = old.ProductCategories.FirstOrDefault(e => e.Id == item.SubCategoryId);
                    old.ProductCategories.Remove(oldSub);
                }

                Update(old);
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

        public IEnumerable<CategoryListViewModel> List(CategoryPageInputViewModel model)
        {
            try
            {
                return Find(e => !e.ParentCategoryId.HasValue, null, "ProductCategories").Select(e => new CategoryListViewModel
                {
                    Id = e.Id,
                    Text = e.Name,
                    Children = e.ProductCategories.Select(c => new LookupViewModel
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

    }
}
