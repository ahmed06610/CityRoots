using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Hubs
{
    public class NotificationHub:Hub
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> OnlineUsers =
            new ConcurrentDictionary<string, HashSet<string>>();
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                OnlineUsers.AddOrUpdate(userId,
                    new HashSet<string> { Context.ConnectionId },
                    (key, oldValue) => { oldValue.Add(Context.ConnectionId); return oldValue; });
            }
            return base.OnConnectedAsync();
            
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                if (OnlineUsers.TryGetValue(userId,out var connections))
                {
                    connections.Remove(Context.ConnectionId);
                    if (connections.Count == 0)
                        OnlineUsers.TryRemove(userId, out _);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}

