using CityRoots.Core.Models.Recommendations;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class InteractionOfMerchantRepository : BaseRepository<InteractionOfMerchant>, IInteractionOfMerchant
    {
        private readonly ApplicationDbContext _context;
        public InteractionOfMerchantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
