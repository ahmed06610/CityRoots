using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CityRoots.Core.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }
        public string Bio {  get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }


        [JsonIgnore] // Ignore to prevent cyclical references
        public virtual List<Farm> Farms { get; set; }
        [JsonIgnore]
        public virtual List<Harvest>Harvests { get; set; }
    }

}
