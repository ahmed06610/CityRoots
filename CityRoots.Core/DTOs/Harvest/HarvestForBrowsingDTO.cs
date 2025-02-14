using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Harvest
{
    public class HarvestForBrowsing
    {
        public int HarvestId { get; set; }  // Unique ID of the harvest
        public string CropName { get; set; }  // Name of the crop
        public string CropType { get; set; }  // Type of the crop (e.g., Fruit, Vegetable)
        public string FarmLocation { get; set; }  // Where the harvest is located
        public double AvailableQuantity { get; set; }  // Quantity available for purchase
        public decimal PricePerUnit { get; set; }  // Price per unit (kg, ton, etc.)
        public DateTime HarvestDate { get; set; }  // When the harvest was collected
        public int Rate { get; set; }  // Farmer's rating (average from users)
        public string ImageUrl { get; set; }  // Optional: Image representing the harvest
        public bool IsBuyer {  get; set; }
    }

}
