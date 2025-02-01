using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class FavoriteFarmers
    {
        public string userId { get; set; }
        public string FarmerId { get; set; }
        [ForeignKey(nameof(FarmerId))]
        public ApplicationUser FarmerUser { get; set; }
    }
}
