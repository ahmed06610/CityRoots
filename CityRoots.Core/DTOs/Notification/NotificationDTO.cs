using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Notification
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string? AdditionalData { get; set; }
    }
}
