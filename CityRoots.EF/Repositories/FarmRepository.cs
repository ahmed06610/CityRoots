using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class FarmRepository : BaseRepository<Farm>, IFarmRepository
    {
        private readonly ApplicationDbContext _context;
        public FarmRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
