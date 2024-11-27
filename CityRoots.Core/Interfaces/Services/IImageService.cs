using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IImageService
    {
        string SaveImage(IFormFile imageFile, string folderPath);
        void DeleteImage(string imagePath);
    }

}
