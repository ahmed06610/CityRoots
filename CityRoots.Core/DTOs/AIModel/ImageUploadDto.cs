using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.AIModel
{
    public class ImageUploadDto
    {
        public IFormFile File { get; set; }
    }
}
