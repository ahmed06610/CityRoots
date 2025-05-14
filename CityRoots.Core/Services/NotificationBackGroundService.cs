using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class NotificationBackGroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICycleNotificationService _cycleNotificationService;
        private readonly IHarvestNotificationService _harvestNotificationService;
        private readonly IScheduleNotificationService _scheduleNotificationService;
        public NotificationBackGroundService(IUnitOfWork unitOfWork, ICycleNotificationService cycleNotificationService, IHarvestNotificationService harvestNotificationService,IScheduleNotificationService scheduleNotificationService)
        {
            _unitOfWork = unitOfWork;
            _cycleNotificationService = cycleNotificationService;
            _harvestNotificationService = harvestNotificationService;
            _scheduleNotificationService = scheduleNotificationService;
        }

        public async Task ProcessNotificationsAsync()
        {
            var cyclesToNotify = await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(null,
                c => c.InvestmentRequests,
                c => c.OpenInvestmentCycle,
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.LandParcel.Farm.Farmer.ApplicationUser);
            foreach (var cycle in cyclesToNotify)
            {
                await _cycleNotificationService.HandleCycleNotificationAsync(cycle);
            }
            /////////////////////////////////////
            var HarvestNotify = await _unitOfWork.Harvest.FindAllWithIncludes<Harvest>(null,
                c => c.Purchases,
                c => c.Farmer,
                c => c.Farmer.ApplicationUser,
                c=>c.Crop
               

                );
            //foreach (var harvest in HarvestNotify)
            //    await _harvestNotificationService.ControlHarvestNotification(harvest.HarvestId);
            ///////////////////Schedules
            var TaskwNotify = await _unitOfWork.Schedule.FindAllWithIncludes<Schedule>(null,
                c => c.Cycle,
                c => c.Cycle.LandParcel,
                 c => c.Cycle.LandParcel.Farm,
                 c => c.Cycle.LandParcel.Farm.Farmer,
                 c => c.Cycle.LandParcel.Farm.Farmer.ApplicationUser

                );
            foreach(var Task in TaskwNotify)
                await _scheduleNotificationService.HandleScheduleNotification(Task);

        }

    }
}
