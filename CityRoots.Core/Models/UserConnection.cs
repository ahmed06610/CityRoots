using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class UserConnection
    {
        [Key]
        public int UserConnectionId { get; set; } // Auto-incrementing Primary Key

        [Required]
        public string UserId { get; set; } // This will store ApplicationUser.Id (the string GUID)

        // Optional: If you want a navigation property back to ApplicationUser
        // [ForeignKey(nameof(UserId))]
        // public virtual ApplicationUser User { get; set; }

        [Required]
        public string ConnectionId { get; set; } // SignalR Connection ID from Context.ConnectionId

        public DateTime ConnectedAt { get; set; }

        public string? UserAgent { get; set; } // Optional: For storing user agent string for info
    }
}
