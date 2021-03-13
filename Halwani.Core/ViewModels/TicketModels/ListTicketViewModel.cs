using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class ListTicketViewModel : PaginationViewModel
    {
        public TicketPageInputFilter Filter { get; set; }
        public TicketPageInputSort Sort { get; set; }
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
