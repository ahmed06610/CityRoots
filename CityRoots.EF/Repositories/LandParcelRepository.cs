using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class LandParcelRepository : BaseRepository<LandParcel>, ILandParcelRepository
    {
        private readonly ApplicationDbContext _context;
        public LandParcelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
