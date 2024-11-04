using CityRoots.Core.CustomValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required, MaxLength(100), MinLength(5), FullName]
        public string Name { get; set; }
        public virtual Farmer Farmer { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual Investor Investor { get; set; }
        // Add the ResetPasswordCode property
        public string? ResetPasswordCode { get; set; }

        // Add the ResetCodeExpiry property
        public DateTime? ResetCodeExpiry { get; set; }
        public string? VerificationCode { get; set; }
      
        public virtual List<Payment> Payments { get; set; }
        public virtual List<Notification> Notifications { get; set; }
        public virtual List<Chat> SentChats { get; set; } // Chats sent by the user
        public virtual List<Chat> ReceivedChats { get; set; } // Chats received by the user
    }

}
