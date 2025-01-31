using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models.Recommendations
{
    public class InteractionOfInvestor
    {
        [Key]
        public int Id { get; set; }
        public int InvestorId { get; set; }
        public int CycleId { get; set; }

    }
}

// all of cycle  (farmerinfo,landinfo,farminfo,cycle)
// history of investor
//fivorite farmers

