using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Crop
{
    public class CropDisplayDto
    {

        public int CropId {  get; set; }
        public string Name { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal ExpectedPriceChange { get; set; }

        public string RiskLevel { get; set; }
        public string cropType { get; set; }
    }
}
