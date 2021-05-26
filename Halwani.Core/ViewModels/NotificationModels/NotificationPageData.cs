using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.NotificationModels
{
    public class NotificationPageData : PageResult<NotificationListViewModel>
    {
        public int UnSeenNotificationsCount { get; set; }
    }
    public class NotificationListViewModel
    {
        public Guid NotificationId { get; set; }
        public DateTime Date { get; set; }
        public string ResourceKey { get; set; }
        public string Text { get; set; }
        public string MadeBy { get; set; }
        public string MadeByName { get; set; }
        public bool IsSeen { get; set; }
        public string ObjectId { get; set; }
        public int NotificationType { get; set; }
    }
    public class NotificationObjectViewModel
    {
        public Guid NotificationId { get; set; }
        public DateTime Date { get; set; }
        public string ResourceKey { get; set; }
        public string MadeBy { get; set; }
        public bool IsSeen { get; set; }
        public string ObjectId { get; set; }
        public int NotificationType { get; set; }
    }
}
