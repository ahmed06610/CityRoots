using CityRoots.Core.Models;

namespace CityRoots.Core.Interfaces
{
    public interface IMerchantRepository : IBaseRepository<Merchant>
    {
        Task<Merchant> GetByAppUserIdAsync(string id);

    }
}
