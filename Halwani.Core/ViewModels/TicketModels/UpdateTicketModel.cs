using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class UpdateTicketModel
    {
        public long Id { get; set; }
        public string Summary { get; set; }
        public string SubmitterTeam { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
        public string TeamName { get; set; }
        public string Location { get; set; }
        public Priority Priority { get; set; }
        public Source Source { get; set; }
        public string ReportedSource { get; set; }
        public int RequestTypeId { get; set; }
        public TicketSeverity TicketSeverity { get; set; }
        public Status TicketStatus { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public string ProductCategoryName1 { get; set; }
        public string ProductCategoryName2 { get; set; }
        public string Attachement { get; set; }

    }
}
