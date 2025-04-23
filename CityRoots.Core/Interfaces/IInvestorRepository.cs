using CityRoots.Core.Models;

namespace CityRoots.Core.Interfaces
{
    public interface IInvestorRepository : IBaseRepository<Investor>
    {
        Task<Investor> GetByAppUserIdAsync(string id);
        Task<List<Investor>> GetInvestorsByIdsAsync(IEnumerable<int> investorIds);

    }
}
