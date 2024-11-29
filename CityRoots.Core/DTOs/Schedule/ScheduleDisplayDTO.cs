using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Schedule
{
    public class ScheduleDisplayDTO
    {
        public int ScheduleId {  get; set; }
        public string _for{ get; set; }
        public int CycleId {  get; set; }
        public string TaskType {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string status {  get; set; }

    }
}
