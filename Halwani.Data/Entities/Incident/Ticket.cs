using Halwani.Data.Entities.SLM_Measurement;
using System;
using System.Collections.Generic;
using System.Text;

using Halwani.Data.Entities.AssoicationTikcet;
using Halwani.Data.Entities.Message;

namespace Halwani.Data.Entities.Incident
{
    public class Ticket : Entity<long>
    {

        public Ticket()
        {
            TicketHistories = new HashSet<TicketHistory>();
            TicketMessage = new HashSet<TicketMessage>();
        }
        #region [Ticker Properties]
        public int TicketNo { get; set; }
        public string TicketName { get; set; }
        public string AssignedUser { get; set; }
        public string SubmitterTeam { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
        public string  Attachement { get; set; }
        public string ServiceName { get; set; }
        public string ReportedSource { get; set; }
        public int RequestTypeId { get; set; }
        public Priority? Priority { get; set; }
        public Source? Source { get; set; }
        public virtual RequestType RequestType { get; set; }
        public TicketSeverity? TicketSeverity { get; set; }
        public Status? TicketStatus { get; set; }
        public string Description { get; set; }
        public string ProductCategoryName1 { get; set; }
        public string ProductCategoryName2 { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
       
        #endregion

        #region [SLM-Measurement]
        public virtual ICollection<SLmMeasurement> SLmMeasurements { get; set; }

        #endregion
        #region messaging
        public virtual ICollection<TicketMessage> TicketMessage { get; set; }
        #endregion

        #region [Assoication]
        public virtual ICollection<Assoication> Assoication { get; set; }
        #endregion

        #region [TicketHistory]
        public virtual ICollection<TicketHistory> TicketHistories{ get; set; }
        #endregion

    }
}
