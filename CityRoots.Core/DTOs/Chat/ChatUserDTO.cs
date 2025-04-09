using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Chat
{
    public class ChatUserDTO
    {
        public string UserId { get; set; }
        public string? UserImageUrl { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        public int UnreadMessages { get; set; }
        public bool IsOnline { get; set; }
        public DateTime DateTimeOfLastMessage { get; set; }
    }
}
