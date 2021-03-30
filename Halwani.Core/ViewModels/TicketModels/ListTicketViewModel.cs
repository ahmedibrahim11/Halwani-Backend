using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Incident;
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
        public int? Location { get; set; }
        public Source? Source { get; set; }
        public Status? State { get; set; }
        public TicketSeverity? Severity { get; set; }
        public Priority? Priority { get; set; }
        public DateTime? Date { get; set; }
    }
    public enum TicketPageInputSort
    {
        CreationDate = 0,
        RasiedBy = 1,
        Topic = 2,
        Severity = 3
    }
}
