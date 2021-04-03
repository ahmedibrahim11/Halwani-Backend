using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class AssignMulipleTicketViewModel
    {
        public long []TicketIds { get; set; }
        public string UserName { get; set; }
    }
}
