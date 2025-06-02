using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace CityRoots.EF.Repositories
{
    public class InvestorRepository : BaseRepository<Investor>, IInvestorRepository
    {
        private readonly ApplicationDbContext _context;
        public InvestorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Investor> GetByAppUserIdAsync(string id)
           => await _context.Investors.SingleOrDefaultAsync(t => t.ApplicationUserId == id);
        public async Task<List<Investor>> GetInvestorsByIdsAsync(IEnumerable<int> investorIds)
        {
            return await _context.Investors
                .Where(investor => investorIds.Contains(investor.InvestorId))
                .ToListAsync();
        }

    }
}
