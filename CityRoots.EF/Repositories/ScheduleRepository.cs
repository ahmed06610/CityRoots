using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
            private readonly ApplicationDbContext _context;
            public ScheduleRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }
    }
}
