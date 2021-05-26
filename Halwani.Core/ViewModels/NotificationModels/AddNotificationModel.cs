using System.Collections.Generic;

namespace Halwani.Core.ViewModels.NotificationModels
{
    public class AddNotificationModel
    {
        public string ResourceKey { get; set; }
        public List<string> UsersIds { get; set; }
        public string ObjectId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
    public enum NotificationType
    {
        NewTicket = 1,
    }
}
