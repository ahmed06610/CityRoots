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
        public NotificationBackGroundService(IUnitOfWork unitOfWork, ICycleNotificationService cycleNotificationService)
        {
            _unitOfWork = unitOfWork;
            _cycleNotificationService = cycleNotificationService;
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

        }

    }
}
