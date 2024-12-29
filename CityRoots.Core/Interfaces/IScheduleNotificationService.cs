using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces
{
    public interface IScheduleNotificationService
    {
        Task HandleScheduleNotification(Schedule schedule);
        Task NotifyRemaningDaysBeforeStart(int scheduleId, string userid);
        Task NotifyStartedTask(int scheduleId,string userid);
        Task NotifyRemaningDaysBeforeEnding(int scheduleId, string userid);
        Task NotifyEndingTask(int scheduleId, string userid); 


    }
}
