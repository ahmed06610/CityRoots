using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.AIModel
{
    public class DiseaseResponseDTO
    {
        public string Name { get; set; }
        public string Diagnosis { get; set; }
        public string Recommendation { get; set; }
        public bool IsIll {  get; set; }
    }
}
