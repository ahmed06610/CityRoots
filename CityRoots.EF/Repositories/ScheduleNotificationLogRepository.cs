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
    public class ScheduleNotificationLogRepository:BaseRepository<ScheduleNotificationLog>,IScheduleNotificationLogRepository
    {
        public ScheduleNotificationLogRepository(ApplicationDbContext context ):base(context)
        {
            
        }
    }
}
