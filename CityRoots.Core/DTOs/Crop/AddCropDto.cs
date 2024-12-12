using CityRoots.Core.Const;
using CityRoots.Core.CustomValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CityRoots.Core.DTOs.Crop
{
    public class AddCropDto
    {
        [Required, UniqueNameCrop, MinLength(1), MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal CurrentPrice { get; set; }
        [Required]
        [Range(1, 1000)]

        public decimal ExpectedPriceChange { get; set; }
        [Required]
        [EnumValidation(typeof(RiskLevel))]
        public string RiskLevel { get; set; }
        [Required, MinLength(3), MaxLength(150)]

        public string RiskDescription { get; set; }
       
        [Required]
        public int CropTypeId {  get; set; }
        public IFormFile Image { get; set; }


    }
}
