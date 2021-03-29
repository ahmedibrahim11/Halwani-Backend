using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.TicketModels
{
    public class UpdateStatusViewModel
    {
        public long TicketId { get; set; }
        public Status Status { get; set; }
    }
}
