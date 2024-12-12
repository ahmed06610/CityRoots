using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Schedule
{
    public class AddScheduleDto
    {
        [Required]
        public int CycleId { get; set; }
        [Required,MinLength(3),MaxLength(15)]

        public string TaskName {  get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        [Required, MinLength(3), MaxLength(150)]

        public string TaskDescription {  get; set; }

    }
}
