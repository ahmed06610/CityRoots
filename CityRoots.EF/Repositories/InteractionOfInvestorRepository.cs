using CityRoots.Core.Models.Recommendations;
using CityRoots.EF.Data;

namespace CityRoots.EF.Repositories
{
    public class InteractionOfInvestorRepository : BaseRepository<InteractionOfInvestor>, IInteractionOfInvestor
    {
        private readonly ApplicationDbContext _context;
        public InteractionOfInvestorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
