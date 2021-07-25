using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class EsclateTicketViewModel
    {
        public long TicketId { get; set; }
        public string EsclationReason { get; set; }
    }
}
