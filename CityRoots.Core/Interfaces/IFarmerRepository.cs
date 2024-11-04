using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces
{
    public interface IFarmerRepository:IBaseRepository<Farmer>
    {
        Task<Farmer> GetByAppUserIdAsync(string id);

    }
}
