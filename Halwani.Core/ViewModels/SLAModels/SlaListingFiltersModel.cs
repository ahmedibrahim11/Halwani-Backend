using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.SLAModels
{
    public class SLAPageInputViewModel : PaginationViewModel
    {
        public SLAModel Filter { get; set; }
        public SLAPageInputSort SortValue { get; set; }
    }

    public enum SLAPageInputSort
    {
        SLAType = 0,
        Priority = 1,
        Team = 2,
        SLAGoal = 3,
        ProductCtaegoryName=4
    }
    public class SLAResultViewModel : PageResult<SLAModel>
    {

    }
}
