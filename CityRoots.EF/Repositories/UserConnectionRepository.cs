using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class UserConnectionRepository : BaseRepository<UserConnection>, IUserConnectionRepository
    {
        private readonly ApplicationDbContext _context;
        public UserConnectionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
