using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class CropRepository : BaseRepository<Crop>, ICropRepository
    {
        private readonly ApplicationDbContext _context;
        public CropRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
