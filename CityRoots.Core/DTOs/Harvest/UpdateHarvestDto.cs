using CityRoots.Core.Const;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.DTOs.Harvest
{
    public class UpdateHarvestDto
    {
        [Required]
        public int HarvestId { get; set; }

        [Required]
        public int CropId { get; set; }
        public int? CycleId { get; set; }
        [Required]
        public double Yield { get; set; }
        [Required]
        public decimal Price{get; set;}
        [Required,EnumDataType(typeof(HarvestStatus))]
        public string status {  get; set; }
        [Required]
        public IFormFile Image { get; set; }

    }
}
