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
    public class CycleNotificationLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CycleNotificationLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HasNotificationBeenSentAsync(int cycleId, CycleNotificationType notificationType, int? investmentRequestId=null)
        {
            if (notificationType != CycleNotificationType.InvestmentRequest)
            {
                return await _unitOfWork.CycleNotificationLog.FindTWithExpression<CycleNotificationLog>(log =>
                    log.CycleId == cycleId && log.CycleNotificationType == notificationType) != null ? true : false;
            }
            else
            {
                return await _unitOfWork.CycleNotificationLog.FindTWithExpression<CycleNotificationLog>(log =>
                     log.CycleId == cycleId && log.CycleNotificationType == notificationType &&
                     log.InvestmentRequestId==investmentRequestId) != null ? true : false;
            }
        }
        public async Task LogNotificationAsync(int cycleId, CycleNotificationType notificationType,int? investmentRequestId=null)
        {
            var log = new CycleNotificationLog
            {
                CycleId = cycleId,
                CycleNotificationType = notificationType,
                NotificationDate = DateTime.UtcNow,
                InvestmentRequestId=notificationType==CycleNotificationType.InvestmentRequest?investmentRequestId : null
                
            };
            await _unitOfWork.CycleNotificationLog.AddAsync(log);
            await _unitOfWork.CompleteAsync();
        }
    }
}
