// In ChatHub.cs
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims; // Add this

// ...
public class ChatHub : Hub<IChatClient>, IChatHub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChatHub> _logger; // Inject logger

    public ChatHub(IUnitOfWork unitOfWork, ILogger<ChatHub> logger) // Add logger
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("OnConnectedAsync: Starting for ConnectionId {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();

        var user = Context.User;
        if (user == null)
        {
            _logger.LogError("OnConnectedAsync: Context.User is NULL for ConnectionId {ConnectionId}. Aborting.", Context.ConnectionId);
            Context.Abort(); // Abort connection if user context is missing
            return;
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            _logger.LogError("OnConnectedAsync: NameIdentifier claim is MISSING for ConnectionId {ConnectionId}. Authenticated User: {UserName}. Aborting.", Context.ConnectionId, user.Identity?.Name);
            Context.Abort();
            return;
        }
        var userId = userIdClaim.Value;
        var connectionId = Context.ConnectionId;

        _logger.LogInformation("OnConnectedAsync: UserId {UserId}, ConnectionId {ConnectionId}", userId, connectionId);

        if (string.IsNullOrEmpty(userId)) // Should be caught by claim check above, but defensive
        {
            _logger.LogWarning("OnConnectedAsync: Resolved UserId is null/empty for ConnectionId {ConnectionId}.", connectionId);
            return; // Or Context.Abort();
        }

        try
        {
            if (_unitOfWork.UserConnection == null)
            {
                _logger.LogError("OnConnectedAsync: _unitOfWork.UserConnection is NULL. DI issue?");
                Context.Abort();
                return;
            }

            _logger.LogInformation("OnConnectedAsync: Querying existing connections for UserId {UserId}", userId);
            var existingUserConnection = await _unitOfWork.UserConnection.FindAllAsync(uc => uc.UserId == userId);
            bool userWasAlreadyOnline = existingUserConnection.Any();
            _logger.LogInformation("OnConnectedAsync: User {UserId} was already online: {WasOnline}", userId, userWasAlreadyOnline);

            string userAgent = "Unknown";
            var httpContext = Context.GetHttpContext();
            if (httpContext?.Request?.Headers != null)
            {
                userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            }
            else
            {
                _logger.LogWarning("OnConnectedAsync: HttpContext or Request or Headers was null when trying to get UserAgent for ConnectionId {ConnectionId}", connectionId);
            }

            var userConnection = new UserConnection
            {
                UserId = userId,
                ConnectionId = connectionId,
                ConnectedAt = DateTime.UtcNow,
                UserAgent = userAgent
            };

            _logger.LogInformation("OnConnectedAsync: Adding UserConnection for UserId {UserId}, ConnectionId {ConnectionId}", userId, connectionId);
            await _unitOfWork.UserConnection.AddAsync(userConnection);
            _logger.LogInformation("OnConnectedAsync: Saving UserConnection (CompleteAsync)");
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("OnConnectedAsync: UserConnection saved for UserId {UserId}", userId);

            if (!userWasAlreadyOnline)
            {
                _logger.LogInformation("OnConnectedAsync: Sending UserStatusChanged (true) for UserId {UserId}", userId);
                await Clients.All.UserStatusChanged(userId, true);
            }
            _logger.LogInformation("OnConnectedAsync: Completed for UserId {UserId}, ConnectionId {ConnectionId}", userId, connectionId);
        }
        catch (NullReferenceException nre)
        {
            _logger.LogError(nre, "OnConnectedAsync: NullReferenceException for ConnectionId {ConnectionId}, UserId {UserId_NRE}", connectionId, userId);
            Context.Abort(); // Abort on critical error
            // DO NOT re-throw here if you want the connection to close gracefully from client's perspective after abort.
            // If you re-throw, SignalR might try to send its own error message, which might also fail.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OnConnectedAsync: General Exception for ConnectionId {ConnectionId}, UserId {UserId_Ex}", connectionId, userId);
            Context.Abort();
            // DO NOT re-throw
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionIdToRemove = Context.ConnectionId;
        _logger.LogInformation(exception, "OnDisconnectedAsync: Starting for ConnectionId {ConnectionId}. Exception: {ExceptionMessage}", connectionIdToRemove, exception?.Message);

        await base.OnDisconnectedAsync(exception); // Call base early if it helps with cleanup context

        try
        {
            if (_unitOfWork.UserConnection == null)
            {
                _logger.LogError("OnDisconnectedAsync: _unitOfWork.UserConnection is NULL. DI issue?");
                return; // Can't do much else
            }

            _logger.LogInformation("OnDisconnectedAsync: Finding UserConnection for ConnectionId {ConnectionId}", connectionIdToRemove);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(uc => uc.ConnectionId == connectionIdToRemove);
            var userConnection = connections.FirstOrDefault();

            if (userConnection != null)
            {
                var userId = userConnection.UserId;
                _logger.LogInformation("OnDisconnectedAsync: Found UserConnection. UserId {UserId}, ConnectionId {ConnectionId_Found}. Removing...", userId, userConnection.ConnectionId);

               await _unitOfWork.UserConnection.DeleteAsync(userConnection); // Ensure this method exists and works
                _logger.LogInformation("OnDisconnectedAsync: Saving deletion (CompleteAsync) for UserId {UserId}", userId);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("OnDisconnectedAsync: Deletion saved for UserId {UserId}", userId);

                _logger.LogInformation("OnDisconnectedAsync: Checking remaining connections for UserId {UserId}", userId);
                var remainingUserConnection = await _unitOfWork.UserConnection.FindAllAsync(uc => uc.UserId == userId);
                bool hasOtherConnections = remainingUserConnection.Any();
                _logger.LogInformation("OnDisconnectedAsync: UserId {UserId} has other connections: {HasOtherConnections}", userId, hasOtherConnections);

                if (!hasOtherConnections)
                {
                    _logger.LogInformation("OnDisconnectedAsync: Sending UserStatusChanged (false) for UserId {UserId}", userId);
                    await Clients.All.UserStatusChanged(userId, false);
                }
                _logger.LogInformation("OnDisconnectedAsync: Completed for UserConnection of UserId {UserId}, ConnectionId {ConnectionId_Completed}", userId, connectionIdToRemove);
            }
            else
            {
                _logger.LogWarning("OnDisconnectedAsync: No UserConnection record found for ConnectionId {ConnectionId_NotFound}. Could not update user status accurately from this event.", connectionIdToRemove);
            }
        }
        catch (NullReferenceException nre)
        {
            _logger.LogError(nre, "OnDisconnectedAsync: NullReferenceException for ConnectionId {ConnectionId_NRE_Disconnect}", connectionIdToRemove);
            // Don't typically abort here as connection is already disconnecting.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OnDisconnectedAsync: General Exception for ConnectionId {ConnectionId_Ex_Disconnect}", connectionIdToRemove);
        }
    }

    // IChatHub methods (SendMessageToUserAsync, IsUserOnlineAsync) from previous response
    public async Task SendMessageToUserAsync(string userId, object messagePayload)
    {
        _logger.LogInformation("SendMessageToUserAsync: Attempting to send to UserId {UserId}", userId);
        if (_unitOfWork?.UserConnection == null)
        {
            _logger.LogError("SendMessageToUserAsync: _unitOfWork.UserConnection is NULL.");
            return;
        }
        var connections = await _unitOfWork.UserConnection.FindAllAsync(uc => uc.UserId == userId);
        var connectionIds = connections.Select(uc => uc.ConnectionId).ToList();

        if (connectionIds.Any())
        {
            _logger.LogInformation("SendMessageToUserAsync: Found {Count} connections for UserId {UserId}. Sending payload.", connectionIds.Count, userId);
            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).ReceiveMessage(messagePayload);
            }
        }
        else
        {
            _logger.LogWarning("SendMessageToUserAsync: User {UserId} has no active connections. Payload not sent in real-time.", userId);
        }
    }

    public async Task<bool> IsUserOnlineAsync(string userId)
    {
        _logger.LogDebug("IsUserOnlineAsync: Checking status for UserId {UserId}", userId);
        if (_unitOfWork?.UserConnection == null)
        {
            _logger.LogError("IsUserOnlineAsync: _unitOfWork.UserConnection is NULL.");
            return false;
        }
        var connections = await _unitOfWork.UserConnection.FindAllAsync(uc => uc.UserId == userId);
        bool isOnline = connections.Any();
        _logger.LogDebug("IsUserOnlineAsync: UserId {UserId} is online: {IsOnline}", userId, isOnline);
        return isOnline;
    }
}