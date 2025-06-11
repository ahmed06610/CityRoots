using CityRoots.Core.CustomValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Auth
{
    public class RegisterDTO
    {

        [Required, MaxLength(50), FullName]
        public string Name { get; set; }
        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required,MinLength(8), MaxLength(100)]
        public string Password { get; set; }
        [Required, RoleDeticated]
        public string Role { get; set; }
        public string? UserName { get; set; }
        public string Bio { get; set; }
    }
}
