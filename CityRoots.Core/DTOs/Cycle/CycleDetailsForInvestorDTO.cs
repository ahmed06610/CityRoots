using CityRoots.Core.DTOs.CycleUpdate;
using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.DTOs.LandParcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.Cycle
{
    public class CycleDetailsForInvestorDTO
    {
        public string CycleName { get; set; }
        public FarmerInfoDTO Farmer { get; set; }
        public LandParcelDTO landParcel { get; set; }
        public CycleDTO InvestmentCycle { get; set; }
        public bool IsInvestorSub {  get; set; }
        public int?  InvestmentRequestId {  get; set; }
        public bool RequestReview {  get; set; }
        public List<CycleUpdateDTO>? cycleUpdates { get; set; }

    }
}
