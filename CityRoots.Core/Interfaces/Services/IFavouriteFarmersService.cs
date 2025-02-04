using CityRoots.Core.DTOs.FavouriteFarmers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IFavouriteFarmersService
    {
        Task<List<FavouriteFarmerDTO>> GetAllFavourites(string userId);
     //   Task<FavouriteFarmerDTO> GetFarmer(string farmerId);    
        Task  AddToFavourites(string FarmerId,string userId);
        Task RemoveFromFavourites(string FarmerId,string userId);

    }
}
