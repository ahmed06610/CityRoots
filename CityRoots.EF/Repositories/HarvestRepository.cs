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

    
        }
    }

