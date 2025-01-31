using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models.Recommendations
{
    public class InteractionOfMerchant
    {
        [Key]
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public int HarvestId { get; set; }
    }
}

// all of harvest  (farmerinfo,landinfo,farminfo,harvest)
// history of merchant
//favorite farmers
