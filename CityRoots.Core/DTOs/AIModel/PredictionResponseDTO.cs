using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.AIModel
{
    public class PredictionResponseDTO
    {
        public string Prediction { get; set; }
        public float Confidence { get; set; }
    }
}
