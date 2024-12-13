using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Purchasereque
{
    public class AllPurchasesRequestForHarvest
    {

        public int PurchaseRequestId { get; set; }
        public string merchantName { get; set; }
        public decimal RequestedPrice { get; set; }
        public double RequestedAmount { get; set; }
        public int harvestId { get; set; }

    }
}
