﻿using Halawani.Core;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<ProductCategory>
    {
        IEnumerable<CategoryListViewModel> List();
        RepositoryOutput Add(List<CreateProductCategroyModel> model);
    }
}
