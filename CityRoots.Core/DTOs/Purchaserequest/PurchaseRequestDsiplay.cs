using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Purchaserequest
{
    public class PurchaseRequestDsiplay
    {
        public int PurchaseRequestId { get; set; }

        public string FarmerName {  get; set; }
       public string HarvestId {  get; set; }
        public string HarvestName {  get; set; }

        public double RequestedAmount { get; set; }


        public decimal RequestedPrice { get; set; }
        public string RequestStatus { get; set; } 

        public DateTime RequestDate { get; set; }
    }
}
