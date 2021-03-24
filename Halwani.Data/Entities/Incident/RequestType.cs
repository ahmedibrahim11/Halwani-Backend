using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Incident
{
    public class RequestType
    {
        public RequestType()
        {
            RequestTypeGroups = new HashSet<RequestTypeGroups>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public TicketType TicketType { get; set; }
        public virtual ICollection<RequestTypeGroups> RequestTypeGroups { get; set; }
    }
}
