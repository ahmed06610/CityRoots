using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace CityRoots.Core.Services
{
    public class ScheduleNotificationService:IScheduleNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ScheduleNotificationLogService _schduleNotificationlogService;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IScheduleService _scheduleService;
        
        public ScheduleNotificationService(IUnitOfWork unitOfWork,ScheduleNotificationLogService scheduleNotificationLogService,INotificationService notificationService,IHubContext<NotificationHub> hubContext,IScheduleService scheduleService) {
        _unitOfWork = unitOfWork;
            _schduleNotificationlogService = scheduleNotificationLogService;
            _notificationService = notificationService;
            _hubContext = hubContext;
            _scheduleService = scheduleService;

        
        }

        public async Task HandleScheduleNotification(Schedule schedule)
        {
            var egyptZone = TZConvert.GetTimeZoneInfo("Africa/Cairo");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);
         

            var FarmerId =schedule.Cycle.LandParcel.Farm.Farmer.ApplicationUser.Id;
            if (schedule.Status != ScheduleStatus.اكتملت.ToString())
            {
                if (now<schedule.StartDate&&
                   now >= schedule.StartDate.AddDays(-1)
                    && schedule.Status == ScheduleStatus.لم_تبدأ.ToString()
                    && !await _schduleNotificationlogService.TaskHasSent(schedule.ScheduleId, ScheduleNotificationType.BeforeStart))
                {
                    await NotifyRemaningDaysBeforeStart(schedule.ScheduleId, FarmerId);
                    await _schduleNotificationlogService.LogTaskNotification(schedule.ScheduleId, ScheduleNotificationType.BeforeStart);
                }
                if (now < schedule.EndDate &&
                    now >= schedule.EndDate.AddDays(-1)  &&
                    !await _schduleNotificationlogService.TaskHasSent(schedule.ScheduleId, ScheduleNotificationType.BeforeEnd) )
                {
                    await NotifyRemaningDaysBeforeEnding(schedule.ScheduleId, FarmerId);
                    await _schduleNotificationlogService.LogTaskNotification(schedule.ScheduleId, ScheduleNotificationType.BeforeEnd);
                }
                if (now>=schedule.StartDate

                    && !await _schduleNotificationlogService.TaskHasSent(schedule.ScheduleId, ScheduleNotificationType.StartedSchedule))
                {
                    await NotifyStartedTask(schedule.ScheduleId, FarmerId);
                    await _schduleNotificationlogService.LogTaskNotification(schedule.ScheduleId, ScheduleNotificationType.StartedSchedule);
                }
                if (now >= schedule.EndDate
                    && !await _schduleNotificationlogService.TaskHasSent(schedule.ScheduleId, ScheduleNotificationType.FinishedSchedule))
                {
                    await NotifyEndingTask(schedule.ScheduleId, FarmerId);
                    await _schduleNotificationlogService.LogTaskNotification(schedule.ScheduleId, ScheduleNotificationType.FinishedSchedule);

                }

            }
        }

        public async Task NotifyEndingTask(int scheduleId, string userid)
        {
            var schedule = await _unitOfWork.Schedule.GetByIdAsync(scheduleId);

            if (schedule is null)
            {
                throw new Exception($"No Tasks With This Id {scheduleId}");
            }
            var content = $"المهمه {schedule.TaskName} رقم {scheduleId} في الدوره  {schedule.Cycle.CycleName} انتهت بالفعل";
            var notification = new CreateNotificationDTO
            {
                Content = content,
                UserId = userid,
                Type = "Schedule",
                AdditionalData = $"ScheduleId : {scheduleId}"
            };
            await _scheduleService.UpdateTheStatus(scheduleId, ScheduleStatus.اكتملت.ToString());
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(userid).SendAsync("ReceiveNotification", notification);


        }

        public async  Task NotifyRemaningDaysBeforeEnding(int scheduleId, string userid)
        {
            var schedule = await _unitOfWork.Schedule.GetByIdAsync(scheduleId);

            if (schedule is null)
            {
                throw new Exception($"No Tasks With This Id {scheduleId}");
            }
            var Content = $"المهمه {schedule.TaskName} رقم {scheduleId} في الدوره {schedule.Cycle.CycleName} ستنتهي في {schedule.EndDate.ToString("dddd hh:mm tt")}  يمكنك تأجيل موعد الانتهاء, اي بعد يوم واحد فقط  ";
            var notification = new CreateNotificationDTO
            {
                Content = Content,
                UserId = userid,
                Type = "Schedule",
                AdditionalData = $"ScheduleId : {scheduleId}"
            };
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(userid).SendAsync("ReceiveNotification", notification);



        }

        public async Task  NotifyRemaningDaysBeforeStart(int scheduleId, string userid)
        {
            var schedule = await _unitOfWork.Schedule.GetByIdAsync(scheduleId);
            if (schedule is null)
            {
                throw new Exception($"No Tasks With This Id {scheduleId}");
            }
            var Content = $"المهمه {schedule.TaskName} رقم {scheduleId} في الدوره {schedule.Cycle.CycleName} ستبدأ في {schedule.StartDate.ToString("dddd hh:mm tt")}  يمكنك تأجيل موعد الابتداء, اي بعد يوم واحد فقط";
            var notification = new CreateNotificationDTO
            {
                Content = Content,
                UserId = userid,
                Type = "Schedule",
                AdditionalData = $"ScheduleId : {scheduleId}"
            };
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(userid).SendAsync("ReceiveNotification", notification);


        }

        public  async Task NotifyStartedTask(int scheduleId, string userid)
        {
            var schedule = await _unitOfWork.Schedule.GetByIdAsync(scheduleId);
            if (schedule is null)
            {
                throw new Exception($"No Tasks With This Id {scheduleId}");
            }
            var content = $"المهمه {schedule.TaskName} رقم {scheduleId} في الدوره {schedule.Cycle.CycleName} بدأت";
            var notification = new CreateNotificationDTO
            {
                Content = content,
                UserId = userid,
                Type = "Schedule",
                AdditionalData = $"ScheduleId : {scheduleId}"
            };
            schedule.Status = ScheduleStatus.في_تقدم.ToString();
            await _scheduleService.UpdateTheStatus(scheduleId, ScheduleStatus.في_تقدم.ToString());
            
            await _notificationService.CreateNotificationAsync(notification);
            await _hubContext.Clients.User(userid).SendAsync("ReceiveNotification", notification);



        }
    }
}
