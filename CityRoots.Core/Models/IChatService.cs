using CityRoots.Core.DTOs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public interface IChatService
    {
        Task SendMessageAsync(string senderId, string receiverId, string message);
        Task<List<Chat>> GetChatMessagesAsync(string senderId, string receiverId);
        Task<List<ChatUserDTO>> GetChatUsersAsync(string userId);
        Task MarkMessagesAsReadAsync(string senderId, string receiverId);
    }
}
