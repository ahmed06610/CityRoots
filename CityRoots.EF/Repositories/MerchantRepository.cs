using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace CityRoots.EF.Repositories
{
    public class MerchantRepository : BaseRepository<Merchant>, IMerchantRepository
    {
        private readonly ApplicationDbContext _context;
        public MerchantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Merchant> GetByAppUserIdAsync(string id)
         => await _context.Merchants.SingleOrDefaultAsync(t => t.ApplicationUserId == id);
    }
}
