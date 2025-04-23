using CityRoots.Core.DTOs.Harvest;
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
       // Task ControlHarvestNotification(int HarvestId);
        Task notifyOnPurchaseRequest(int HarvestId,int merchantId,PurchaseRequest request);
        Task NotifyFinishedYield(HarvestNotificationDto harvest);
        Task NotifyMerchantOfpurchaseResponseAsync(string farmerName, HarvestNotificationDto harvest, string status);
    }
}
