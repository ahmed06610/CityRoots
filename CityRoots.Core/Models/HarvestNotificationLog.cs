using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class HarvestNotificationLog
    {
        public int Id { get; set; }
        public string ForWho {  get; set; }
        public HarvestNotificationType HarvestNotificationType {  get; set; }
        public int HarvestId {  get; set; }
        public Harvest harvest { get; set; }
        public DateTime NotificationDate { get; set; }
        public int? PurchaseRequestId { get; set; }


    }
}
