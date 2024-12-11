using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.EF.Repositories
{
    public class AiPredictRepository : BaseRepository<AiPredict>, IAiPredictRepository
    {
        private readonly ApplicationDbContext _context;
        public AiPredictRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
