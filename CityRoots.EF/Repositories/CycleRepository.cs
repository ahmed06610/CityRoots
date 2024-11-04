using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class CycleRepository : BaseRepository<Cycle>, ICycleRepository
    {
        private readonly ApplicationDbContext _context;
        public CycleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
