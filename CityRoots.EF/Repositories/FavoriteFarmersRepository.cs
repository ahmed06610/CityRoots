using CityRoots.Core.Models;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class FavoriteFarmersRepository : BaseRepository<FavoriteFarmers>, IFavoriteFarmers
    {
        private readonly ApplicationDbContext _context;
        public FavoriteFarmersRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
