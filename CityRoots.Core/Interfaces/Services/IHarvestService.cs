using CityRoots.Core.DTOs.Harvest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IHarvestService
    {
        Task<IEnumerable<HarvestDisplayDto>> GetAll(string s=null);
        Task<HarvestDisplayDto> Get(int id);
        Task<AddHarvestDto> Add(AddHarvestDto harvest);
        Task<UpdateHarvestDto> Update(UpdateHarvestDto harvest);
        Task Delete(int id);
    }
}
