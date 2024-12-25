using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface ICycleNotificationService
    {
        Task NotifyInvestmentRequestAsync(int cycleId, string userId, string investorName, decimal amount);
        Task NotifyInvestmentGoalMetAsync(int cycleId, string userId);
        Task NotifyInsufficientInvestmentAsync(int cycleId, string userId, DateTime startDate);
        Task NotifyCycleStartedAsync(int cycleId, string userId);
        Task NotifyCycleEndApproachingAsync(int cycleId, string userId, DateTime endDate);
        Task HandleCycleNotificationAsync(Cycle cycle);
    }
}
