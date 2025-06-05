using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CityRoots.Core.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var connection = new UserConnection
                {
                    UserId = userId,
                    ConnectionId = Context.ConnectionId,
                    ConnectedAt = DateTime.UtcNow,
                    UserAgent = "unknown"
                };

               await _unitOfWork.UserConnection.AddAsync(connection);
                await _unitOfWork.CompleteAsync();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            var connection = await _unitOfWork.UserConnection.FindTWithExpression<UserConnection>(x=>x.ConnectionId==connectionId);

            if (connection != null)
            {
                await _unitOfWork.UserConnection.DeleteAsync(connection);
                await _unitOfWork.CompleteAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
