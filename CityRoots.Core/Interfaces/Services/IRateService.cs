using CityRoots.Core.DTOs.Rate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IRateService
    {
        Task MakeTheRating(RateRequest rate, string userId);
        Task DeleteTheRating(DeleteRate rate, string userId); 
    }
}
