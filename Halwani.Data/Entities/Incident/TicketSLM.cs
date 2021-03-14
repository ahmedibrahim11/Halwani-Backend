using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Incident
{
    public class TicketSLM : Entity<long>
    {
        public long TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public SLMType SLMType { get; set; }
        public SLAStatus SLAStatus { get; set; }
        public int SLADuration { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
