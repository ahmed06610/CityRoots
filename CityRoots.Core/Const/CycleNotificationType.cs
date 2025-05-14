using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Const
{
    public enum CycleNotificationType
    {
        InvestmentRequest,       // Investment request notification
        InvestmentGoalMet,       // Investment goal met notification
        InsufficientInvestment,  // Insufficient investment notification
        CycleStarted,            // Cycle start notification
        CycleEndApproaching,     // Cycle end approaching notification
        cycleEnded
    }
}
