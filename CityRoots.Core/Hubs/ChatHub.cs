using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CityRoots.Core.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> OnlineUsers =
            new ConcurrentDictionary<string, HashSet<string>>();
        /* public async Task SimulateOnConnected(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                OnlineUsers[userId] = Context.ConnectionId;
                await Clients.All.SendAsync("UserStatusChanged", userId, true);
            }
        }*/
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, DateTime.UtcNow);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                OnlineUsers.AddOrUpdate(userId,
                    new HashSet<string> { Context.ConnectionId },
                    (key, oldValue) => { oldValue.Add(Context.ConnectionId); return oldValue; });
            }

            await Clients.All.SendAsync("UserStatusChanged", userId, true);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                if (OnlineUsers.TryGetValue(userId, out var connections))
                {
                    connections.Remove(Context.ConnectionId);
                    if (connections.Count == 0)
                        OnlineUsers.TryRemove(userId, out _);
                }
            }

            await Clients.All.SendAsync("UserStatusChanged", userId, false);
            await base.OnDisconnectedAsync(exception);
        }

        public static List<string> GetOnlineUserIds()
        {
            return OnlineUsers.Keys.ToList();
        }
    }
}
