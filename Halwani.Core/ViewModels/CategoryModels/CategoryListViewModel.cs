using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.CategoryModels
{
    public class CategoryListViewModel : LookupViewModel
    {
  
        public IEnumerable<LookupViewModel> Children { get; set; }
    }
}
