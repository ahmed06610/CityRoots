using CityRoots.Core.Models;

namespace CityRoots.Core.Interfaces
{
    public interface IHarvestRepository : IBaseRepository<Harvest>
    {
        Task<IEnumerable<Harvest>> GetAllWithIncludes(string Name=null);
        Task<Harvest> GetWithInclude(int Id);
    }
}
