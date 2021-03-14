using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Incident
{
   public class TicketHistory:Entity<long>
    {
        public long TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string FromTeam { get; set; }
        public string ToTeam { get; set; }
        public Status OldStatus { get; set; }
        public Status NewStatus { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
