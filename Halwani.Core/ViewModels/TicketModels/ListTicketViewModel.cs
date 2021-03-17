using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class TicketPageInputViewModel  : PaginationViewModel
    {
        public TicketPageInputFilter Filter { get; set; }
        public TicketPageInputSort SortValue { get; set; }
    }
    public class TicketPageInputFilter
    {
    }
    public enum TicketPageInputSort
    {
        CreationDate = 0,
        RasiedBy = 1,
        Topic = 2,
        Severity = 3
    }
}
