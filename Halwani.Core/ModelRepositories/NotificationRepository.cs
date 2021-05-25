using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.NotificationModels;
using Halwani.Data.Entities.Notification;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halwani.Core.ModelRepositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public NotificationRepository(IUserRepository userRepository, IConfiguration configuration) : base()
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<RepositoryOutput> Add(AddNotificationModel model, string loggedUserId, string token)
        {
            try
            {
                var notificationObjectViewModel = new NotificationObjectViewModel
                {
                    Date = DateTime.Now,
                    IsSeen = false,
                    MadeBy = loggedUserId,
                    NotificationId = Guid.NewGuid(),
                    NotificationType = (int)model.NotificationType,
                    ObjectId = model.ObjectId,
                    ResourceKey = model.ResourceKey
                };
                foreach (var userId in model.UsersIds)
                {
                    AddNewNotification(loggedUserId, notificationObjectViewModel, userId);
                }
                if (Save() < model.UsersIds.Count)
                    return RepositoryOutput.CreateErrorResponse("");

                await SendSignalR(token, "updateNotification", model.UsersIds.ToArray());
                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public NotificationPageData List(PaginationViewModel model, string userId)
        {
            try
            {
                var result = new NotificationPageData();
                var userNotification = Find(e => !e.IsDeleted && e.UserId == userId).FirstOrDefault();
                if (userNotification == null)
                    return new NotificationPageData();

                var query = JsonConvert.DeserializeObject<List<NotificationObjectViewModel>>(userNotification.NotificationBody);
                result.TotalCount = query.Count;
                result.UnSeenNotificationsCount = query.Count(e => !e.IsSeen);

                SeeNotificationsInPage(model, userNotification, query);
                result.PageData = FormatPageData(model, query);
                return result;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public async Task SendSignalR(string token, string eventName,/* List<string> userIds,*/ params string[] paramters)
        {
            //Note:get token without bearer
            try
            {
                var signalrUrl = _configuration["SignalR:Url"];
                //var signalrUrl = "https://localhost:44312/hubs";
                var hubConnectionBuilder = new HubConnectionBuilder();
                var hubConnection = hubConnectionBuilder.WithUrl(signalrUrl, Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets, options =>
                {
                    options.AccessTokenProvider = async () => { return token; };
                    options.UseDefaultCredentials = true;
                }).Build();
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync(eventName, paramters);
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
            }
        }

        #region Private

        private void AddNewNotification(string loggedUserId, NotificationObjectViewModel notificationListViewModel, string userId)
        {
            var oldNotification = Find(e => !e.IsDeleted && e.UserId == userId).FirstOrDefault();
            if (oldNotification == null)
            {
                Add(new Notification
                {
                    UserId = userId,
                    NotificationBody = JsonConvert.SerializeObject(new List<NotificationObjectViewModel> { notificationListViewModel }),
                    CreatedBy = loggedUserId,
                    CreationDate = DateTime.Now
                });
            }
            else
            {
                EditUserExistingNotification(notificationListViewModel, oldNotification);
            }
        }

        private void EditUserExistingNotification(NotificationObjectViewModel notificationListViewModel, Notification oldNotification)
        {
            var oldNotificationData = JsonConvert.DeserializeObject<List<NotificationObjectViewModel>>(oldNotification.NotificationBody);
            oldNotificationData.Add(notificationListViewModel);
            oldNotification.NotificationBody = JsonConvert.SerializeObject(oldNotificationData);
            Update(oldNotification);
        }

        private void HandleMadeByName(List<NotificationListViewModel> query)
        {
            foreach (var item in query)
            {
                var user = _userRepository.GetById(item.MadeBy);
                item.MadeByName = user.Name;
            }
        }

        private void SeeNotificationsInPage(PaginationViewModel model, Notification userNotification, List<NotificationObjectViewModel> query)
        {
            var index = 0;
            foreach (var item in query)
            {
                item.IsSeen = true;
                if (index < model.PageSize)
                    index++;
                else
                    break;
            }
            userNotification.NotificationBody = JsonConvert.SerializeObject(query);
            Update(userNotification);
            Save();
        }

        private List<NotificationListViewModel> FormatPageData(PaginationViewModel model, List<NotificationObjectViewModel> query)
        {
            var pageData = query.OrderByDescending(e => e.Date).Skip(model.PageSize * model.PageNumber).Take(model.PageSize).Select(e => new NotificationListViewModel
            {
                Date = e.Date,
                ResourceKey = e.ResourceKey,
                IsSeen = e.IsSeen,
                MadeBy = e.MadeBy,
                NotificationId = e.NotificationId,
                NotificationType = e.NotificationType,
                ObjectId = e.ObjectId
            }).ToList();
            HandleMadeByName(pageData);
            return pageData;
        }

        #endregion
    }
}
