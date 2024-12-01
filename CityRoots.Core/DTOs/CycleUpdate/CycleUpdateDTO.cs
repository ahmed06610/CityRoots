using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.CycleUpdate
{
    public class CycleUpdateDTO
    {
        public int UpdateId { get; set; }
        public int CycleId { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal GrowthRate { get; set; }
        public string QualityCheck { get; set; }
        public string ImageUrl { get; set; }
        public string AdditionalNotes { get; set; }
    }
}
