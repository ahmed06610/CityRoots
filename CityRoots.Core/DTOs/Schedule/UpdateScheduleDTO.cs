using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Schedule
{
    public class UpdateScheduleDTO:AddScheduleDto
    {
        public int ScheduleId { get; set; }

    }
}
