using CityRoots.Core.Const;
using CityRoots.Core.CustomValidation;
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
        [Required]
        [EnumValidation(typeof(CropType))]
        public string CropType { get; set; }
    }
}
