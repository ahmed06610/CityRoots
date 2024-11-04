using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class InvestmentRequestRepository : BaseRepository<InvestmentRequest>, IInvestmentRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public InvestmentRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
