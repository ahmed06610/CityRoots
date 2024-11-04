using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace CityRoots.EF.Repositories
{
    public class FarmerRepository : BaseRepository<Farmer>, IFarmerRepository
    {
        private readonly ApplicationDbContext _context;
        public FarmerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Farmer> GetByAppUserIdAsync(string id)
           => await _context.Farmers.SingleOrDefaultAsync(t => t.ApplicationUserId == id);
    }
}
