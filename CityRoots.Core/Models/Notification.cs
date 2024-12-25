using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Type { get; set; } // e.g., Schedule, Cycle, Harvest

        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        public string? AdditionalData { get; set; } // JSON or serialized data for extra details
    }
}
