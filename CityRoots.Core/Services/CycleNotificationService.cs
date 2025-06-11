using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Notification;
using CityRoots.Core.Hubs;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.SignalR;
using TimeZoneConverter;

namespace CityRoots.Core.Services
{
    public class CycleNotificationService : ICycleNotificationService
    {
        private readonly INotificationService _notificationService;
        private readonly ICycleService _cycleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CycleNotificationLogService _cycleNotificationLogService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CycleNotificationService(
            INotificationService notificationService,
            ICycleService cycleService,
            IUnitOfWork unitOfWork,
            CycleNotificationLogService cycleNotificationLogService,
            IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _cycleService = cycleService;
            _unitOfWork = unitOfWork;
            _cycleNotificationLogService = cycleNotificationLogService;
            _hubContext = hubContext;
        }

        public async Task HandleCycleNotificationAsync(Cycle cycle)
        {
            var egyptZone = TZConvert.GetTimeZoneInfo("Africa/Cairo");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);

            var farmerId = cycle.LandParcel.Farm.Farmer.ApplicationUser.Id;
           
            var investorsId = cycle.InvestmentRequests.
                Where(x => x.RequestStatus == InvestmentStatues.مقبول.ToString())
                .Select(x=>x.InvestorId)
                .Distinct()
                .ToList();
            

           

            // Notify when investment goal is met before the cycle start date
            if (cycle.OpenInvestmentCycle != null &&
                cycle.OpenInvestmentCycle.CurrentTotalInvestment >= cycle.OpenInvestmentCycle.ExpectedFinancialGoal &&
                now < cycle.StartDate &&
                !await _cycleNotificationLogService.HasNotificationBeenSentAsync(cycle.CycleId, CycleNotificationType.InvestmentGoalMet))
            {
                await NotifyInvestmentGoalMetAsync(cycle.CycleId, farmerId);
                await _cycleNotificationLogService.LogNotificationAsync(cycle.CycleId, CycleNotificationType.InvestmentGoalMet);
            }

            // Notify if the cycle start is approaching and the investment goal is insufficient
            if (cycle.OpenInvestmentCycle != null &&
                now.AddDays(2) >= cycle.StartDate &&
                cycle.OpenInvestmentCycle.CurrentTotalInvestment < cycle.OpenInvestmentCycle.ExpectedFinancialGoal &&
                !await _cycleNotificationLogService.HasNotificationBeenSentAsync(cycle.CycleId, CycleNotificationType.InsufficientInvestment))
            {
                await NotifyInsufficientInvestmentAsync(cycle.CycleId, farmerId, cycle.StartDate);
                await _cycleNotificationLogService.LogNotificationAsync(cycle.CycleId, CycleNotificationType.InsufficientInvestment);
            }

            // Notify when the cycle starts
            if (now.Date >= cycle.StartDate.Date &&
                !await _cycleNotificationLogService.HasNotificationBeenSentAsync(cycle.CycleId, CycleNotificationType.CycleStarted))
            {
                await NotifyCycleStartedAsync(cycle.CycleId, farmerId);
                var investors = await _unitOfWork.Investor.GetInvestorsByIdsAsync(investorsId);
                foreach (var investor in investors)
                {

                    await NotifyCycleStartedAsync(cycle.CycleId, investor.ApplicationUserId);
                
                    
                }
            }

            // Notify if the cycle end is approaching
            if (now.AddDays(2) >= cycle.EndDate &&
                !await _cycleNotificationLogService.HasNotificationBeenSentAsync(cycle.CycleId, CycleNotificationType.CycleEndApproaching))
            {
                await NotifyCycleEndApproachingAsync(cycle.CycleId, farmerId, cycle.EndDate);
                await _cycleNotificationLogService.LogNotificationAsync(cycle.CycleId, CycleNotificationType.CycleEndApproaching);
            }

            //Notify if the cycle ended
            if (now.Date >= cycle.EndDate.Date &&
               !await _cycleNotificationLogService.HasNotificationBeenSentAsync(cycle.CycleId, CycleNotificationType.cycleEnded))
            {
                await NotifyCycleEndedAsync(cycle.CycleId, farmerId);
                await _cycleNotificationLogService.LogNotificationAsync(cycle.CycleId, CycleNotificationType.cycleEnded);
                var investors = await _unitOfWork.Investor.GetInvestorsByIdsAsync(investorsId);
                foreach (var investor in investors)
                {

                    await NotifyCycleEndedAsync(cycle.CycleId, investor.ApplicationUserId);


                }
            }

        }

        //public async Task NotifyInvestmentRequestAsync(int cycleId, int investorId, decimal amount)
        //{
        //    var investor = await _unitOfWork.Investor.FindTWithIncludes<Investor>(investorId, "InvestorId", x => x.ApplicationUser);
        //    var investorName = investor.ApplicationUser.Name;
        //    var cycle = await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(cycleId, "CycleId",
        //        x => x.LandParcel,
        //        x => x.LandParcel.Farm,
        //        x => x.LandParcel.Farm.Farmer,
        //        x => x.LandParcel.Farm.Farmer.ApplicationUser);
        //    var userId = cycle.LandParcel.Farm.Farmer.ApplicationUserId;
        //    var content = $"استثمار جديد من {investorName} بمبلغ {amount} في الدورة {cycle.CycleName} (رقم {cycleId}).";
        //    var notification = new CreateNotificationDTO
        //    {
        //        UserId = userId,
        //        Content = content,
        //        Type = "Cycle",
        //        AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
        //    };

        //    await _notificationService.CreateNotificationAsync(notification);
        //    await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

        //}
        public async Task NotifyInvestmentRequestAsync(int cycleId, string userId, int investorId, decimal amount)
        {
            var investor = await _unitOfWork.Investor.FindTWithIncludes<Investor>(investorId, "InvestorId", x => x.ApplicationUser);
            var investorName = investor.ApplicationUser.Name;
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"استثمار جديد من {investorName} بمبلغ {amount} في الدورة {cycle.CycleName} (رقم {cycleId}).";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "الدوره الزراعيه",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
          //  await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

        }

        public async Task NotifyInvestmentGoalMetAsync(int cycleId, string userId)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"هدف الاستثمار للدورة {cycle.CycleName} (رقم {cycleId}) قد تحقق! يمكنك بدء الدورة الآن.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "الدوره الزراعيه",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
            // await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

        }

        public async Task NotifyInsufficientInvestmentAsync(int cycleId, string userId, DateTime startDate)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var daysLeft = (startDate - DateTime.Now).TotalDays;
            var content = $"الدورة \"{cycle.CycleName}\" (رقم {cycleId}) ستبدأ خلال {daysLeft:F0} يومًا، ولكن لم يتم الوصول إلى هدف الاستثمار بعد. يُمكنك تأجيل موعد البدء إذا لزم الأمر.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "الدوره الزراعيه",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
            //await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

        }

        public async Task NotifyCycleStartedAsync(int cycleId, string userId)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"الدورة {cycle.CycleName} (رقم {cycleId}) قد بدأت الآن.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "الدوره الزراعيه",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
           // await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

        }

        public async Task NotifyCycleEndApproachingAsync(int cycleId, string userId, DateTime endDate)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var daysLeft = (endDate - DateTime.Now).TotalDays;
            var content = $"الدورة \"{cycle.CycleName}\" (رقم {cycleId}) ستنتهي خلال {daysLeft:F0} يومًا. يمكنك تأجيل موعد الانتهاء إذا لزم الأمر.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "الدوره الزراعيه",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
           // await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);

        }

        public async Task NotifyInvestorOfInvestmentResponseAsync(int cycleId, string FarmerName, int investorId, string status)
        {
            var cycle=await _cycleService.GetCycleByIdAsync(cycleId);
            var investor=await _unitOfWork.Investor.GetByIdAsync(investorId);
            var content = status == InvestmentStatues.مقبول.ToString() ?
                $"لقد تم قبول طلبك من قبل {FarmerName} بشأن الاستثمار في دوره {cycle.CycleName} رقم {cycleId}" :
                $"لقد تم رفض طلبك من قبل {FarmerName} بشأن الاستثمار في دوره {cycle.CycleName} رقم {cycleId}";
           
            var userId = investor.ApplicationUserId;
            var notification = new CreateNotificationDTO
            {
                Type = "طلب استثمار",
                Content = content,
                UserId = userId,
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"



            };
            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }



        }
        public async Task NotifyCycleEndedAsync(int cycleId, string userId)
        {
            var cycle = await _cycleService.GetCycleByIdAsync(cycleId);
            var content = $"الدورة {cycle.CycleName} (رقم {cycleId}) قد انتهت.";
            var notification = new CreateNotificationDTO
            {
                UserId = userId,
                Content = content,
                Type = "الدوره الزراعيه",
                AdditionalData = $"{{ \"CycleId\": {cycleId} }}"
            };

            await _notificationService.CreateNotificationAsync(notification);
            var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == userId);

            if (connections.Any())
            {
                foreach (var conn in connections)
                {
                    await _hubContext.Clients.Client(conn.ConnectionId)
                        .SendAsync("ReceiveNotification", notification);
                }
            }
        }

        public async Task NotifyInvestorOnCyclesUpdates(int cycleId, string farmerName)
        {

            var cycle = await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(cycleId, "CycleId", x => x.InvestmentRequests);

            var investorsId = cycle.InvestmentRequests.
                           Where(x => x.RequestStatus == InvestmentStatues.مقبول.ToString())
                           .Select(x => x.InvestorId)
                           .Distinct()
                           .ToList();
            var investors = await _unitOfWork.Investor.GetInvestorsByIdsAsync(investorsId);
            var content = $"قام {farmerName} بإضافة تحديث بخصوص الدورة {cycle.CycleName} (رقم {cycleId}).";

            foreach (var investor in investors)
            {
                var notification = new CreateNotificationDTO
                {
                    Type = "تحديث الدورة الزراعية",
                    UserId = investor.ApplicationUserId,
                    AdditionalData = $"{{ \"CycleId\": {cycleId} }}",
                    Content = content

                };
                await _notificationService.CreateNotificationAsync(notification);
                var connections = await _unitOfWork.UserConnection.FindAllAsync(x => x.UserId == investor.ApplicationUserId);

                if (connections.Any())
                {
                    foreach (var conn in connections)
                    {
                        await _hubContext.Clients.Client(conn.ConnectionId)
                            .SendAsync("ReceiveNotification", notification);
                    }
                }

            }

        }

        public async Task NotifyInvestorOnUpdateOncycle(int cycleId,string userName)
        {
            var cycle = await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(cycleId, "CycleId", x => x.InvestmentRequests);

            var investorsId = cycle.InvestmentRequests.
                           Where(x => x.RequestStatus == InvestmentStatues.مقبول.ToString())
                           .Select(x => x.InvestorId)
                           .Distinct()
                           .ToList();
            var investors = await _unitOfWork.Investor.GetInvestorsByIdsAsync(investorsId);
            var content = $"تم تعديل في الدوره رقم {cycleId} من قبل {userName}.";
            foreach (var investor in investors)
            {
                var notification = new CreateNotificationDTO
                {
                    Type = "تحديث الدورة الزراعية",
                    UserId = investor.ApplicationUserId,
                    AdditionalData = $"{{ \"CycleId\": {cycleId} }}",
                    Content = content

                };
                await _notificationService.CreateNotificationAsync(notification); 
                var connections = await _unitOfWork.UserConnection.FindAllAsync(x=>x.UserId== investor.ApplicationUserId);

                if (connections.Any())
                {
                    foreach (var conn in connections)
                    {
                        await _hubContext.Clients.Client(conn.ConnectionId)
                            .SendAsync("ReceiveNotification", notification);
                    }
                }



            }


        }
        

    }
}
