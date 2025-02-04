using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.FavouriteFarmers
{
    public class FavouriteFarmerDTO
    {
        public string userId {  get; set; }
        public string FarmerId { get; set; }
        public string Name { get; set; }
        public string Email {  get; set; }
        public string bio {  get; set; }
        public string phoneNumber {  get; set; }
    }
}
