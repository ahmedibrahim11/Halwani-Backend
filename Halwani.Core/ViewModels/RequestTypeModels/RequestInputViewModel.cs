using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.RequestTypeModels
{
    public class RequestTypeInputViewModel : PaginationViewModel
    {
        public RequestTypeInputFilter Filter { get; set; }
        public RequestTypeInputSort SortValue { get; set; }
    }
    public class RequestTypeInputFilter
    {
        public string Team { get; set; }

        public int? GroupID { get; set; }

        public TicketSeverity? Severity { get; set; }
        public TicketType? TicketType { get; set; }
        public string SearchText { get; set; }
        public Priority? Priority { get; set; }

    }
    public enum RequestTypeInputSort
    {
        Title = 0,
        Group = 1,
        TicketType = 2,
        Team = 3
    }
    public class RequestTypeResultViewModel : PageResult<RequestTypeData>
    {
     
    }
    public class RequestTypeData
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public int TicketType { get; set; }
        public string Team { get; set; }

        public bool IsVisable { get; set; }
    }
    public class GetRequestType    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public TicketSeverity Sevirity { get; set; }
        public Priority Priority { get; set; }
        public int TicketType { get; set; }
        public string Team { get; set; }
        public List<int> GroupIDs { get; set; }
        public string Icon { get; set; }

    }
}
