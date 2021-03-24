using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class TicketPageResultViewModel : PageResult<TicketPageData>
    {
        public bool CanAdd { get; set; }
    }
    public class TicketPageData
    {
        public long ID { get; set; }
        public RasiedByViewModel RasiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string TicketTopic { get; set; }
        public RequestTypeModel RequestType { get; set; }
        public TicketSeverity? Severity { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAssign { get; set; }
    }
    public class RequestTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public TicketType TicketType { get; set; }
    }
    public class RasiedByViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
