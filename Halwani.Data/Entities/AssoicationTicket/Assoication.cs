using System;
using System.Collections.Generic;
using System.Text;
using Halwani.Data.Entities.Incident;

namespace Halwani.Data.Entities.AssoicationTikcet
{
   public class Assoication:Entity<long>
    {
        public long TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Sumbitter { get; set; }
        public string FormName { get; set; }
        public string DependandForm { get; set; }
        public string DependandRequestId { get; set; }



    }
}
