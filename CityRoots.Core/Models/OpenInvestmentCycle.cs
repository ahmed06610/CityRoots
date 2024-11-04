using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class OpenInvestmentCycle
    {
        [Key]
        public int OpenInvestmentCycleId { get; set; }

        [Required]
        public int CycleId { get; set; }

        [ForeignKey(nameof(CycleId))]
        public virtual Cycle Cycle { get; set; }

        [Required]
        public decimal ExpectedFinancialGoal { get; set; }
        public decimal MinimumInvestment { get; set; }
        public decimal MaximumInvestment { get; set; }
        public int MaxInvestorsAllowed { get; set; }
        public int CurrentInvestorCount { get; set; }
        public decimal CurrentTotalInvestment { get; set; }
    }

}
