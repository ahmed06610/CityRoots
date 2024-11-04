using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ResetCode { get; set; }
        [Required, PasswordPropertyText]
        public string NewPassword { get; set; }
    }
}
