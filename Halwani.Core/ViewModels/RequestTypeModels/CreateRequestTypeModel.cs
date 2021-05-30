using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.RequestTypeModels
{
   public class CreateRequestTypeModel
    {
        public int? Id { get; set;}
        public string Name { get; set;}
        public string Icon { get; set;}
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public int[] GroupIds { get; set;}
        public string TeamName { get; set; }
        public Priority Priority { get; set; }
        public TicketSeverity Severity { get; set; }
        
    }
}
