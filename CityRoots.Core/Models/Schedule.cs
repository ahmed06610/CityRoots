using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class Schedule
    {
        public Schedule()
        {
            Status = "Pending";
        }
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int CycleId { get; set; }

        [ForeignKey(nameof(CycleId))]
        public virtual Cycle Cycle { get; set; }

        [Required]
        public string TaskName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // Pending, In Progress, Completed
        public string TaskDescription { get; set; }

    }

}
