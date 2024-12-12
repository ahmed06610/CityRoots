using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class CycleUpdate
    {
        [Key]
        public int UpdateId { get; set; }

        [Required]
        public int CycleId { get; set; }

        [ForeignKey(nameof(CycleId))]
        public virtual Cycle Cycle { get; set; }

        public DateTime UpdateDate { get; set; }
        public decimal GrowthRate { get; set; }
        public string QualityCheck { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AdditionalNotes { get; set; }
    }

}
