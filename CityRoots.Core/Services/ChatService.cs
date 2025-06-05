using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Chat;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.Core.Hubs; // For IChatClient
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CityRoots.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;

        public ChatService(IUnitOfWork unitOfWork, IHubContext<ChatHub, IChatClient> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        private async Task<bool> IsUserOnlineAsync(string userId) // Helper method within the service
        {
            var connections = await _unitOfWork.UserConnection.FindAllAsync(uc => uc.UserId == userId);
            return connections.Any();
        }

        public async Task SendMessageAsync(string senderId, string receiverId, string messageContent)
        {
            // 1. Save the chat message
            var chat = new Chat
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                MessageContent = messageContent,
                Timestamp = TimeHelper.NowInEgypt,
                IsRead = false
            };
            await _unitOfWork.Chat.AddAsync(chat);
            await _unitOfWork.CompleteAsync();

            // 2. Prepare the payload
            var messagePayload = new
            {
                ChatId = chat.ChatId.ToString(),
                SenderId = senderId,
                ReceiverId = receiverId,
                MessageContent = messageContent,
                Timestamp = chat.Timestamp
            };

            // 3. Send to receiver if online
            if (await IsUserOnlineAsync(receiverId))
            {
                // Using IHubContext to send to a specific user's connections.
                // SignalR's IUserIdProvider (defaulting to NameIdentifier claim) handles mapping userId to connection(s).
                await _hubContext.Clients.User(receiverId).ReceiveMessage(messagePayload);
            }
            else
            {
                Console.WriteLine($"ChatService.SendMessageAsync: Receiver {receiverId} is offline. Message saved. (Consider push notifications here).");
                // Here you would integrate with your _pushNotificationService if the user is offline,
                // similar to your NotificationService.
                // var tokens = await _deviceTokenRepository.GetTokensByUserIdAsync(receiverId);
                // if (tokens != null && tokens.Any())
                // {
                //    await _pushNotificationService.SendPushNotificationAsync(tokens, "New Message", messageContent);
                // }
            }

            // 4. Optional: Send to sender's other connections for UI sync
            // Be cautious with this if the client already optimistically updates its UI.
            await _hubContext.Clients.User(senderId).ReceiveMessage(messagePayload);
        }

        public async Task<List<Chat>> GetChatMessagesAsync(string senderId, string receiverId)
        {
            return (await _unitOfWork.Chat
                .FindAllAsync(c =>
                    (c.SenderId == senderId && c.ReceiverId == receiverId) ||
                    (c.SenderId == receiverId && c.ReceiverId == senderId)))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public async Task<List<ChatUserDTO>> GetChatUsersAsync(string userId)
        {
            var messages = await _unitOfWork.Chat.FindAllWithIncludes<Chat>(
                c => c.SenderId == userId || c.ReceiverId == userId,
                c => c.Sender,
                c => c.Receiver
            );

            // Get online status from UserConnections table
            var allDbConnections = await _unitOfWork.UserConnection.FindAllAsync(uc => true);
            List<string> onlineUserIds = allDbConnections.Select(uc => uc.UserId).Distinct().ToList();

            var users = messages
                .Select(m => m.SenderId == userId ? m.Receiver : m.Sender)
                .Where(u => u != null && u.Id != userId)
                .DistinctBy(u => u.Id)
                .Select(otherUser =>
                {
                    var lastMessageWithThisUser = messages
                        .Where(m => (m.SenderId == userId && m.ReceiverId == otherUser.Id) || (m.SenderId == otherUser.Id && m.ReceiverId == userId))
                        .OrderByDescending(m => m.Timestamp)
                        .FirstOrDefault();

                    return new ChatUserDTO
                    {
                        UserId = otherUser.Id,
                        UserName = otherUser.UserName,
                        UserImageUrl = otherUser.ImageProfileUrl,
                        LastMessage = lastMessageWithThisUser?.MessageContent,
                        UnreadMessages = messages.Count(m => m.ReceiverId == userId && m.SenderId == otherUser.Id && !m.IsRead),
                        DateTimeOfLastMessage = lastMessageWithThisUser?.Timestamp,
                        IsOnline = onlineUserIds.Contains(otherUser.Id)
                    };
                })
                .OrderByDescending(dto => dto.DateTimeOfLastMessage ?? DateTime.MinValue)
                .ToList();
            return users;
        }

        public async Task MarkMessagesAsReadAsync(string userid, string Senderid)
        {
            var unreadMessages = await _unitOfWork.Chat.FindAllAsync(c =>
                c.SenderId == Senderid && c.ReceiverId == userid && !c.IsRead);

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                }
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}