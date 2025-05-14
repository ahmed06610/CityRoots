using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.Purchaserequest;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IPurchaseRequestService
    {
        Task<List<PurchaseRequestDsiplay>> GetAllRequestsForHarvest(int HarvestId);
        Task<List<PurchaseRequestDsiplay>> GetAllRequestsForMerchant(int MerchantId);
        Task<PurchaseRequest> GetSpecificRequest(int RequestId);
        Task<PurchaseRequest> CreatePurchaseRequest(CreatePurchaseRrquest purchaseRrquest,int merchantId);
        Task Delete(int RequestId);
        Task<HarvestNotificationDto> UpdateRequest(int requestId, string status);  
    }
}
