using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class OpenInvestmentCycleRepository : BaseRepository<OpenInvestmentCycle>, IOpenInvestmentCycleRepository
    {
        private readonly ApplicationDbContext _context;
        public OpenInvestmentCycleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
