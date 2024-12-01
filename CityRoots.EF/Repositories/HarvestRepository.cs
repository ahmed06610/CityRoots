using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CityRoots.EF.Repositories
{
    public class HarvestRepository : BaseRepository<Harvest>, IHarvestRepository
    {
        private readonly ApplicationDbContext _context;
        public HarvestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Harvest>> GetAllWithIncludes(string Name = null)
        {
            var harvests =  _context.Harvests
                .Include(x => x.Cycle)
                .ThenInclude(x => x.CycleUpdates)
                .Include(x => x.Cycle)
                .ThenInclude(x => x.LandParcel)
                .ThenInclude(x=>x.Farm)
                .Include(x => x.Farmer)
                .ThenInclude(x => x.ApplicationUser)
                .Include(x => x.Crop);
            if (Name != null) {
                harvests.Where(x => x.Crop.Name.Contains(Name)); 
            }
                
           
            return await harvests.ToListAsync();
        }

        public async Task<Harvest> GetWithInclude(int Id)
        {
            var harvest = _context.Harvests.Include(x => x.Crop)
               .Include(x => x.Cycle)
               .ThenInclude(x => x.CycleUpdates)
               .Include(x => x.Cycle)
               .ThenInclude(x => x.LandParcel)
               .Include(x => x.Farmer)
               .ThenInclude(x => x.ApplicationUser);
            return await harvest.FirstOrDefaultAsync(x=>x.HarvestId == Id);
        }
    }
}
