using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public string MessageContent { get; set; }

        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }

}
