using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class InvestmentRequest
    {
        [Key]
        public int InvestmentRequestId { get; set; }

        [Required]
        public int CycleId { get; set; }

        [ForeignKey(nameof(CycleId))]
        public virtual Cycle Cycle { get; set; }

        [Required]
        public int InvestorId { get; set; }

        [ForeignKey(nameof(InvestorId))]
        public virtual Investor Investor { get; set; }

        [Required]
        public decimal RequestedAmount { get; set; }

        [Required]
        public string RequestStatus { get; set; } // Pending, Approved, Rejected

        public DateTime RequestDate { get; set; }
        public string RequestedProfitType { get; set; } // Cash, Crop Share
    }

}
