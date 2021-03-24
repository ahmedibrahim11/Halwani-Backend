using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.RequestTypeModels
{
   public class CreateRequestTypeModel
    {
        public string Name { get; set;}
        public string Icon { get; set;}
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public int[] GroupIds { get; set;}
    }
}
