using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IHarvestNotificationService
    {
        Task ControlHarvestNotification(int HarvestId);
        Task notifyOnPurchaseRequest(int HarvestId, string UserId, string merchantname, PurchaseRequest request);
        Task NotifyFinishedYield(int HarvestId, string UserId);
    }
}
