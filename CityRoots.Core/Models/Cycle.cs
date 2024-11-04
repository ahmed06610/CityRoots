using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class Cycle
    {
        [Key]
        public int CycleId { get; set; }

        [Required]
        public int ParcelId { get; set; }

        [ForeignKey(nameof(ParcelId))]
        public virtual LandParcel LandParcel { get; set; }

        [Required]
        public int CropId { get; set; }

        [ForeignKey(nameof(CropId))]
        public virtual Crop Crop { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double ExpectedYield { get; set; }

        public string AvailableProfitTypes { get; set; } // Cash, Crop Share, Both

        // Navigation Properties
        public virtual OpenInvestmentCycle OpenInvestmentCycle { get; set; }
        public virtual List<InvestmentRequest> InvestmentRequests { get; set; }
        public virtual List<Schedule> Schedules { get; set; }
        public virtual List<CycleUpdate> CycleUpdates { get; set; }
    }

}
