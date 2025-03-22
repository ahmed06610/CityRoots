using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Helpers
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Get the "sub" claim (User ID from JWT) and use it as the SignalR user identifier
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? connection.User?.FindFirst("sub")?.Value;
        }
    }
}
