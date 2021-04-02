﻿using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class TicketDetailsModel
    {

        public int TicketNo { get; set; }
        public string TicketName { get; set; }
        public string ResolveText { get; set; }
        public string AssignedUser { get; set; }
        public string SubmitterTeam { get; set; }
        public string SubmitterEmail { get; set; }
        public string SubmitterName { get; set; }
        public string[] Attachement { get; set; }
        public string TeamName { get; set; }
        public string ReportedSource { get; set; }
        public int RequestTypeId { get; set; }
        public Priority? Priority { get; set; }
        public Source? Source { get; set; }
        public RequestTypeModel RequestType { get; set; }
        public TicketSeverity? TicketSeverity { get; set; }
        public Status? TicketStatus { get; set; }
        public string Description { get; set; }
        public string ProductCategoryName1 { get; set; }
        public string ProductCategoryName2 { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

}
