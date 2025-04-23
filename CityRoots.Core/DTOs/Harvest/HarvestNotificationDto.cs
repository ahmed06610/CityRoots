using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Harvest
{
    public class HarvestNotificationDto
    {
        public string status {  get; set; }
        public int HarvestId {  get; set; }
        public string cropName {  get; set; }
        public string userId {  get; set; }
        public string merchantId {  get; set; }
    }
}
