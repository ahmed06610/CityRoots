using CityRoots.Core.DTOs.Chat;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace CityRoots.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;  // Use IHubContext

        public ChatService(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(string senderId, string receiverId, string message)
        {
            var chat = new Chat
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                MessageContent = message,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            await _unitOfWork.Chat.AddAsync(chat);
            await _unitOfWork.CompleteAsync();

            // Send real-time update correctly
            await _hubContext.Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, DateTime.UtcNow);


        }

        public async Task<List<Chat>> GetChatMessagesAsync(string senderId, string receiverId)
        {
            return (await _unitOfWork.Chat
                .FindAllAsync(c =>
                    (c.SenderId == senderId && c.ReceiverId == receiverId) ||
                    (c.SenderId == receiverId && c.ReceiverId == senderId))).OrderBy(m => m.Timestamp).ToList();
        }

        public async Task<List<ChatUserDTO>> GetChatUsersAsync(string userId)
        {
            var messages = await _unitOfWork.Chat.FindAllWithIncludes<Chat>(
                c => c.SenderId == userId || c.ReceiverId == userId,
                c => c.Sender,
                c => c.Receiver
            );
            var onlineUsers = ChatHub.GetOnlineUserIds();  // Get all online users


            var users = messages
                .GroupBy(c => c.SenderId == userId ? c.Receiver : c.Sender)
                .Select(g => new ChatUserDTO
                {
                    UserId = g.Key.Id,
                    UserName = g.Key.UserName,
                    LastMessage = g.OrderByDescending(c => c.Timestamp).FirstOrDefault()?.MessageContent,
                    UnreadMessages = g.Count(m => !m.IsRead && m.ReceiverId == userId),
                    DateTimeOfLastMessage = g.OrderByDescending(c => c.Timestamp).FirstOrDefault().Timestamp,
                    IsOnline = onlineUsers.Contains(g.Key.Id) // Check once, not inside loop
                })
                .ToList();

            return users;
        }

        public async Task MarkMessagesAsReadAsync(string senderId, string receiverId)
        {
            var unreadMessages = await _unitOfWork.Chat.FindAllAsync(c =>
                c.SenderId == senderId && c.ReceiverId == receiverId && !c.IsRead);

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
