    using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Helpers
{
    public class DateHelper
    {
        public static string FormatArabicDate(DateTime date)
        {
            CultureInfo arCulture = new CultureInfo("ar-EG");
            return date.ToString("dddd hh:mm tt", arCulture);
        }
        public static string FormatArabicFullDate(DateTime date)
        {
            CultureInfo arCulture = new CultureInfo("ar-EG");
            return date.ToString("dddd، dd MMMM yyyy، HH:mm", arCulture);
        }
    }
}
