using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.SLM_Measurement
{
    public class SLmMeasurement:Entity<long>
    {
        public long  TicketId { get; set; }
        public long SLAId { get; set; }
        public string SLAStatus { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
