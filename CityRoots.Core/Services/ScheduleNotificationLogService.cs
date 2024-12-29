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
    public class ScheduleNotificationLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleNotificationLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public async Task<bool> TaskHasSent(int ScheduleId,ScheduleNotificationType scheduleNotificationType)
        {
            return await _unitOfWork.ScheduleNotificationLog.FindTWithExpression<ScheduleNotificationLog>(
                s=>s.scheduleId==ScheduleId && s.scheduleNotificationType==scheduleNotificationType)!=null?true:false;
        }
        public async Task LogTaskNotification(int ScheduleId,ScheduleNotificationType scheduleNotificationType)
        {
            var Notificationlog = new ScheduleNotificationLog()
            {
                scheduleNotificationType = scheduleNotificationType,
                scheduleId = ScheduleId,
                NotificationDate = DateTime.UtcNow,

            };
            await _unitOfWork.ScheduleNotificationLog.AddAsync(Notificationlog);
            await _unitOfWork.CompleteAsync();
          
        }
    }
}
