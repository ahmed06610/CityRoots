using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.OpenInvestmentCycle
{
    public class UpdateOpenInvestmentCycleDTO:CreateOpenInvestmentCycleDTO
    {
        public int OpenInvestmentCycleId { get; set; }
    }
}
