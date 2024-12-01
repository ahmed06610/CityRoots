using CityRoots.Core.DTOs.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDisplayDTO>> GetAll(int CycleId);
        Task<ScheduleDisplayDTO> Get(int Id);
        Task<ScheduleDisplayDTO> Add(AddScheduleDto schedule);
        Task<ScheduleDisplayDTO> update(UpdateScheduleDTO schedule);
        Task Delete(int Id);
        Task CompelteTask(int Id);
    }
}
