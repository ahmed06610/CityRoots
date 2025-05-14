using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IFavoriteFarmerNotificationService
    {
        Task NotifyOnFavoriteList(string userName, string farmerId);
    }
}
