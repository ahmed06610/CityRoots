using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class CycleNotificationLog
    {
        [Key]
        public int NotificationLogId { get; set; }

        [Required]
        public int CycleId { get; set; }

        [ForeignKey(nameof(CycleId))]
        public Cycle Cycle { get; set; }

        [Required]
        public CycleNotificationType CycleNotificationType { get; set; } // Use enum here

        [Required]
        public DateTime NotificationDate { get; set; }
        public int? InvestmentRequestId { get; set; }
    }
}
