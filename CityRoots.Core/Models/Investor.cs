using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.Models
{
    public class Investor
    {
        [Key]
        public int InvestorId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public string Bio {  get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Navigation Properties
        public virtual List<InvestmentRequest> InvestmentRequests { get; set; }
    }

}
