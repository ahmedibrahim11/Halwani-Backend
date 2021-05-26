using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.NotificationModels;
using Halwani.Data.Entities.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        #region General Methods

        #endregion

        #region Custom Methouds
        Task<RepositoryOutput> Add(AddNotificationModel model, string loggedUserId, string token);
        NotificationPageData List(PaginationViewModel model, string userId);
        Task SendSignalR(string token, string eventName,/* List<string> userIds,*/ params string[] paramters);
        #endregion
    }
}
