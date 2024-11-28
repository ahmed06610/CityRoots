using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Cycle
{
    public class UpdateCycleDTO:CreateCycleDTO
    {
        public int CycleId { get; set; }
    }
}
