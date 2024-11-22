using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Models
{
        public class FeedBack
        {
        public FeedBack()
        {
            Date = DateTime.Now;
        }


        public int Id { get; set; }
            [Required]
            public int Rate { get; set; }
            [Required]
            public string Descripition {  get; set; }
            public DateTime Date { get; private set; }
            [ForeignKey(nameof(User))]
            public string? UserId {  get; set; }
            public ApplicationUser? User { get; set; }
         


        }
}
