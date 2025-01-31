using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
    public class FavoriteFarmers
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public string FarmerId { get; set; }
    }
}
