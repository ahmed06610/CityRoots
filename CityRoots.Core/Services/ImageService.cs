using Microsoft.AspNetCore.Hosting;

namespace CityRoots.Core.Services
{
    using CityRoots.Core.Interfaces.Services;
    using Microsoft.AspNetCore.Http;

    using System.IO;

    public class ImageService : IImageService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment  _webHostEnvironment;

        public ImageService(IHostingEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string SaveImage(IFormFile imageFile, string folderPath)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

            var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return Path.Combine(folderPath, uniqueFileName).Replace("\\", "/"); // Return relative path
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

}
