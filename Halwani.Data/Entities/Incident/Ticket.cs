using Halwani.Data.Entities.SLM_Measurement;
using System;
using System.Collections.Generic;
using System.Text;

using Halwani.Data.Entities.AssoicationTikcet;

namespace Halwani.Data.Entities.Incident
{
    public class Ticket : Entity<long>
    {
        #region [Ticker Properties]
        public int TicketNo { get; set; }
        public string TicketName { get; set; }
        public string SubmiotterTeam { get; set; }
        public string SubmiotterEmail { get; set; }
        public string ServiceName { get; set; }
        public string ReportedSource { get; set; }
        public TicketType TicketType { get; set; }
        public Status TicketStatus { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime ResolvedDate { get; set; }
        public DateTime LastModifiedDate { get; set; } 
        #endregion

        #region [SLM-Measurement]
        public virtual ICollection<SLmMeasurement> SLmMeasurements { get; set; }

        #endregion

        #region [Assoication]
        public virtual ICollection<Assoication> MyProperty { get; set; }
        #endregion

    }
}
