using CityRoots.Core.Const;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class HarvestNotificationLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HarvestNotificationLogService(IUnitOfWork unitOfWork) { 
        _unitOfWork = unitOfWork;
        }
        public async Task<bool> HarvestNotificationhassent(int harvestid,HarvestNotificationType harvestNotificationType,int? PurchaseRequestId=null )
        {
            if (harvestNotificationType != HarvestNotificationType.PurchaseRequest)
                return await _unitOfWork.HarvestNotificationLog.FindTWithExpression<HarvestNotificationLog>(
                    x => x.HarvestId == harvestid && x.HarvestNotificationType == harvestNotificationType) != null ? true : false;
            else
                return await _unitOfWork.HarvestNotificationLog.FindTWithExpression<HarvestNotificationLog>(
                    x => x.HarvestId == harvestid && x.HarvestNotificationType == harvestNotificationType && x.PurchaseRequestId==PurchaseRequestId) != null ? true : false;

        }
        public async Task logHarvestNotification(int harvestid, HarvestNotificationType harvestNotificationType,string forwho ,int? PurchaseRequestId = null)
        {
            var notification = new HarvestNotificationLog
            {
                HarvestId = harvestid,
                HarvestNotificationType = harvestNotificationType,
                PurchaseRequestId=harvestNotificationType==HarvestNotificationType.PurchaseRequest? PurchaseRequestId:null,
                ForWho = forwho,
                NotificationDate=DateTime.UtcNow,
                
                
            
            };
            await _unitOfWork.HarvestNotificationLog.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

        }
    }
}
