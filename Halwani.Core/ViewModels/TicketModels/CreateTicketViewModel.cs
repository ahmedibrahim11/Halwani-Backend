using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
   public class CreateTicketViewModel
    {
        
        public string Summary { get; set; }
        public string SubmitterTeam { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
        public string ServiceName { get; set; }
        public string ReportedSource { get; set; }
        public TicketType Type { get; set; }
        public TicketSeverity TicketSeverity { get; set; }
        public Status TicketStatus { get; set; }
        public string Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public string ProductCategoryName1 { get; set; }
        public string ProductCategoryName2 { get; set; }
        public string Attachement { get; set; }
    }
}
