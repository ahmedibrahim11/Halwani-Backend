using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Message
{
   public class TicketMessage : Entity<long>
    {
        public string MessageText { get; set; }
        public string Submitter { get; set; }
        public long TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
