using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces
{
    public interface IChatHub
    {
        Task SendMessageToUserAsync(string userId, object messagePayload); // Send a pre-formatted payload
        Task<bool> IsUserOnlineAsync(string userId);
        // Add other methods if your service needs to interact with the hub in more ways
    }
    public interface IChatClient
    {
        Task ReceiveMessage(object messagePayload);
        Task UserStatusChanged(string userId, bool isOnline);
        // Add any other methods that your server will invoke on the client
    }
}
