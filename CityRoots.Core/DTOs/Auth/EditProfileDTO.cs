using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Auth
{
    public class EditProfileDTO
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public IFormFile? Image { get; set; } // Add this for profile image
    }
}
