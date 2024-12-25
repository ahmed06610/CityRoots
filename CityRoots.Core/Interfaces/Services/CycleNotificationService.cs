using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace CityRoots.Core.Interfaces.Services
{
    public class CycleNotificationService : ICycleNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly ICycleService _cycleService; // Assume this service provides access to cycle details.
        private readonly IUnitOfWork _unitOfWork;

        public CycleNotificationService(INotificationService notificationService, ICycleService cycleService, IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _cycleService = cycleService;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleCycleNotificationAsync(Cycle cycle)
        {
            // Convert current time to Egypt timezone
            var egyptZone = TZConvert.GetTimeZoneInfo("Africa/Cairo");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);

            var farmerId = cycle.LandParcel.Farm.Farmer.ApplicationUser.Id;

            // Notify pending investment requests
            if (cycle.InvestmentRequests.Any(i => i.RequestStatus == InvestmentStatues.Pending.ToString()))
            {
                foreach (var investment in cycle.InvestmentRequests.Where(i => i.RequestStatus == InvestmentStatues.Pending.ToString()))
                {
                    var investorName = (await _unitOfWork.Investor.FindTWithIncludes<Investor>(
                        investment.InvestorId,
                        "InvestorId",
                        i => i.ApplicationUser)).ApplicationUser.UserName;

                    await NotifyInvestmentRequestAsync(cycle.CycleId, farmerId, investorName, investment.RequestedAmount);
                }
            }

            // Notify when investment goal is met before the cycle start date
            if (cycle.OpenInvestmentCycle!=null &&cycle.OpenInvestmentCycle.CurrentTotalInvestment >= cycle.OpenInvestmentCycle.ExpectedFinancialGoal
                && now < cycle.StartDate)
            {
                await NotifyInvestmentGoalMetAsync(cycle.CycleId, farmerId);
            }

            // Notify if the cycle start is approaching and the investment goal is insufficient
            if (cycle.OpenInvestmentCycle != null && now.AddDays(2) >= cycle.StartDate
                && cycle.OpenInvestmentCycle.CurrentTotalInvestment < cycle.OpenInvestmentCycle.ExpectedFinancialGoal)
            {
                await NotifyInsufficientInvestmentAsync(cycle.CycleId, farmerId, cycle.StartDate);
            }

            // Notify when the cycle starts
            if (now.Date == cycle.StartDate.Date)
            {
                await NotifyCycleStartedAsync(cycle.CycleId, farmerId);
            }

            // Notify if the cycle end is approaching
            if (now.AddDays(2) >= cycle.EndDate)
            {
                await NotifyCycleEndApproachingAsync(cycle.CycleId, farmerId, cycle.EndDate);
            }
        }
        public async Task NotifyInvestmentRequestAsync(int cycleId, string userId, string investorName, decimal amount)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"استثمار جديد من {investorName} بمبلغ {amount} في الدورة {cycle.CycleName} (رقم {cycleId}).";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "Cycle",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
        }

        public async Task NotifyInvestmentGoalMetAsync(int cycleId, string userId)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"هدف الاستثمار للدورة {cycle.CycleName} (رقم {cycleId}) قد تحقق! يمكنك بدء الدورة الآن.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "Cycle",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
        }

        public async Task NotifyInsufficientInvestmentAsync(int cycleId, string userId, DateTime startDate)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var daysLeft = (startDate - DateTime.Now).TotalDays;
            var content = $"الدورة {cycle.CycleName} (رقم {cycleId}) ستبدأ في {daysLeft:F0} أيام ولكن لم يكتمل هدف الاستثمار. يمكنك تأجيل موعد البدء.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "Cycle",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
        }

        public async Task NotifyCycleStartedAsync(int cycleId, string userId)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"الدورة {cycle.CycleName} (رقم {cycleId}) قد بدأت الآن.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "Cycle",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
        }

        public async Task NotifyCycleEndApproachingAsync(int cycleId, string userId, DateTime endDate)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var daysLeft = (endDate - DateTime.Now).TotalDays;
            var content = $"الدورة {cycle.CycleName} (رقم {cycleId}) ستنتهي في {daysLeft:F0} أيام. يمكنك تأجيل موعد الانتهاء إذا لزم الأمر.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "Cycle",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
        }
    }
}
