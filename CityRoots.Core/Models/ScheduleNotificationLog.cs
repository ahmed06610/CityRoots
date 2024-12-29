using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class ScheduleNotificationLog
    {
        public int id {  get; set; }
        public ScheduleNotificationType scheduleNotificationType { get; set; }
        public DateTime NotificationDate { get; set; }
            public int scheduleId {  get; set; }
            [ForeignKey(nameof(scheduleId))]
            public Schedule Schedule { get; set; }


    }
}
