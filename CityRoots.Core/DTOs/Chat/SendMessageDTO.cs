using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Chat
{
    public class SendMessageDTO
    {
        public string ReceiverId { get; set; }
        public string Message { get; set; }
    }
}
