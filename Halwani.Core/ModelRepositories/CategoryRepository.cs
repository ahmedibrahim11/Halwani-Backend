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
                    IsVisible = e.IsVisible,
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
                foreach (var item in model.SubCategory.Where(e => !e.SubCategoryId.HasValue || e.SubCategoryId == 0))
                {
                    old.ProductCategories.Add(new ProductCategory
                    {
                        Name = item.SubCategoryName,
                        Goal = item.Goal
                    });
                }
                foreach (var item in model.SubCategory.Where(e => e.SubCategoryId.HasValue && !e.IsDeleted))
                {
                    var oldSub = old.ProductCategories.FirstOrDefault(e => e.Id == item.SubCategoryId);
                    if (oldSub != null)
                    {
                        oldSub.Name = item.SubCategoryName;
                        oldSub.Goal = item.Goal;
                    }

                }
                foreach (var item in model.SubCategory.Where(e => e.SubCategoryId.HasValue && e.IsDeleted))
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

        public CreateProductCategroyModel GetForEdit(long ID)
        {
            var category = Find(r => r.Id == ID).FirstOrDefault();
            return new CreateProductCategroyModel()
            {
                ParentCategoryId = category.Id,
                ParentCategory = category.Name,
                SubCategory = category.ProductCategories.Select(e => new SubCateogryModel
                {
                    Goal = e.Goal,
                    SubCategoryId = e.Id,
                    SubCategoryName = e.Name
                }).ToList()
            };
        }

        public CategoryResultViewModel List(CategoryPageInputViewModel model, out RepositoryOutput response)
        {
            try
            {
                response = new RepositoryOutput();
                CategoryResultViewModel result = new CategoryResultViewModel();

                var qurey = Find(r => r.Parent == null, null, "");

                qurey = FilterList(model, qurey);
                qurey = SortList(model, qurey);
                PagingList(model, result, qurey);

                return result;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                response = RepositoryOutput.CreateErrorResponse(ex.Message);
                return null;
            }
        }

        #region Private

        private IEnumerable<ProductCategory> FilterList(CategoryPageInputViewModel model, IEnumerable<ProductCategory> query)
        {
            if (model.SearchText.Length != 0)
                return query.Where(r => r.Name.Contains(model.SearchText[0]));
            return query;
        }
        private IEnumerable<ProductCategory> SortList(CategoryPageInputViewModel model, IEnumerable<ProductCategory> query)
        {
            return query;
        }
        private void PagingList(CategoryPageInputViewModel model, CategoryResultViewModel result, IEnumerable<ProductCategory> qurey)
        {
            result.TotalCount = qurey.Count();
            if (!model.IsPrint)
                qurey = qurey.Skip(model.PageNumber * model.PageSize).Take(model.PageSize);

            foreach (var item in qurey)
            {
                result.PageData.Add(new CategoryListViewModel
                {
                    Id = item.Id,
                    Text = item.Name,
                    Children = item.ProductCategories.Select(e => new LookupViewModel
                    {
                        Id = e.Id,
                        Text = e.Name,
                        Goal = e.Goal

                    }),
                    IsVisible = item.IsVisible
                });
            }
        }

        public RepositoryOutput UpdateVisiblity(int id, bool isVisible)
        {
            try
            {
                var RT = Find(e => e.Id == id).FirstOrDefault();
                RT.IsVisible = isVisible;
                Update(RT);
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

        #endregion
    }
}
