using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces
{
    public interface IRateNotificationService
    {
        Task NotifyOnRating(string userName, string farmerId, int Rate);
    }
}
