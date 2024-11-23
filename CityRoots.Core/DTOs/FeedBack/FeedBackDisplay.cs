using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.DTOs.FeedBack
{
    public class FeedBackDisplay
    {
        public int id {  get; set; }
        public int Rate { get; set; }
        public string Descripition { get; set; }
        public DateTime Date { get; set; }
        public string UserName {  get; set; }

    }
}
