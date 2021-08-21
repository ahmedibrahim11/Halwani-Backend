using Halwani.Core.ViewModels.GenericModels;

namespace Halwani.Core.ViewModels.CategoryModels
{
    public class CategoryPageInputViewModel  : PaginationViewModel
    {
        public CategoryPageInputFilter Filter { get; set; }
        public CategoryPageInputSort? SortValue { get; set; }
    }
    public class CategoryPageInputFilter
    {
        
    }
    public enum CategoryPageInputSort
    {
        Name = 0
    }

    public class CategoryResultViewModel : PageResult<CategoryListViewModel>
    {

    }
}
