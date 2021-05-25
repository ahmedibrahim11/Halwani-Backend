using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Notification
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string UserId { get; set; }
        public string NotificationBody { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
    }
}
