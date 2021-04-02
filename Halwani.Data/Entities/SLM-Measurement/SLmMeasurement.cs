using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.SLM_Measurement
{
    public class SLmMeasurement:Entity<long>
    {
        public long  TicketId { get; set; }
        public long SLAId { get; set; }
        public SLAStatus SLAStatus { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual SLA.SLA SLA { get; set; }
    }
}
