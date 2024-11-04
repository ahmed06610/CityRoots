using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class CycleUpdateRepository : BaseRepository<CycleUpdate>, ICycleUpdateRepository
    {
        private readonly ApplicationDbContext _context;
        public CycleUpdateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
