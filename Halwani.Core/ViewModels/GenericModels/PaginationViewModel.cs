using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.GenericModels
{
    public class PaginationViewModel
    {
        public string SearchText { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool IsPrint { get; set; }
    }
}
