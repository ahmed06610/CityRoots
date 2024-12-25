using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.CycleNotification
{
    public class CycleNotificationDTO
    {
        public int CycleId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; } = NotificationsTypes.CycleNotification.ToString();
        public string? AdditionalData { get; set; }
    }
}
