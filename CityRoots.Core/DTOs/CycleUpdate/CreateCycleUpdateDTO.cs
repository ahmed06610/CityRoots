using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.CycleUpdate
{
    public class CreateCycleUpdateDTO
    {
        public int CycleId { get; set; }
        public decimal GrowthRate { get; set; }
        public string QualityCheck { get; set; }
        public IFormFile Image { get; set; }
        public string AdditionalNotes { get; set; }
    }
}
