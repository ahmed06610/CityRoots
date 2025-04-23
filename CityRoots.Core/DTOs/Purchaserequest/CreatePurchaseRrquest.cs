using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Purchaserequest
{
    public class CreatePurchaseRrquest
    {
        public int HarvestId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]

        public double RequestedAmount { get; set; }
        public decimal requestedPrice {  get; set; }
    
        public string Notes { get; set; }
    }
}
