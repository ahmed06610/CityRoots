using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Auth
{
    public class ProfileInfoDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public int? Rate { get; set; }
        public string? ImageProfileUrl { get; set; }
    }
}

