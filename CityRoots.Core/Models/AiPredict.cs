using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class AiPredict
    {
        public int AiPredictId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string? Diagnosis { get; set; }
        public string? Recommendation { get; set; }
        public bool IsIll {  get; set; }
    }
}
