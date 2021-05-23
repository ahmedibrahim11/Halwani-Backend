using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Halwani.Hubs
{
    public class SignalRUsers
    {
        public ClaimsPrincipal Claims { get; set; }
        public string UserId { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
    public interface ITypedHubClient
    {
        Task send(string name, string message);
        Task InvokeAsync(string v, object id);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HubBase : Hub
    {
        public static IHubContext<HubBase> Current { get; set; }

        public HubBase()
        {

        }
        private static readonly HashSet<string> ConnectedIds = new HashSet<string>();
        public static readonly ConcurrentDictionary<string, SignalRUsers> Users
            = new ConcurrentDictionary<string, SignalRUsers>();
        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = Users.GetOrAdd(userId, _ => new SignalRUsers
            {
                UserId = userId,
                //Claims = (ClaimsPrincipal) Context.User.Identity,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string connectionId = Context.ConnectionId;

            SignalRUsers user;
            Users.TryGetValue(userId, out user);

            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                    if (!user.ConnectionIds.Any())
                    {
                        Users.TryRemove(userId, out SignalRUsers removedUser);
                    }
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void updateNotification(List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                if (!Users.ContainsKey(userId))
                    continue;
                var dicUser = Users[userId];
                if (dicUser != null)
                {
                    foreach (var connectionId in dicUser.ConnectionIds)
                    {
                        if (connectionId == Context.ConnectionId)
                            continue;
                        Clients.Client(connectionId).SendAsync("changeNotificationCount");
                    }
                }
            }
        }

        public void updateTickets(List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                if (!Users.ContainsKey(userId))
                    continue;
                var dicUser = Users[userId];
                if (dicUser != null)
                {
                    foreach (var connectionId in dicUser.ConnectionIds)
                    {
                        if (connectionId == Context.ConnectionId)
                            continue;
                        Clients.Client(connectionId).SendAsync("updateTickets");
                    }
                }
            }
        }

    }
}
