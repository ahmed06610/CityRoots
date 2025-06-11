using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Schedule;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace CityRoots.Core.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private static readonly TimeZoneInfo egyptZone = TZConvert.GetTimeZoneInfo("Africa/Cairo");
        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;

        }
        public async Task<ScheduleDisplayDTO> Add(AddScheduleDto schedule)
        {
            if(schedule is null) throw new ArgumentNullException(nameof(schedule), "Schedule data is required");
            if (schedule.StartDate > schedule.EndDate) throw new Exception("لا يمكن ان يكون موعد بدايه المهمه اكبر من موعد نهايه المهمه");
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(schedule.CycleId);
            if (cycle is null)
                 throw new Exception($"No Cycle with Id {schedule.CycleId}");
            if (schedule.StartDate < cycle.StartDate || schedule.EndDate > cycle.EndDate)
                throw new Exception($"لا يمكن ان يكون موعد بدايه او نهايه المهمه خارج فتره الدوره المحدده من {cycle.StartDate} الي {cycle.EndDate})");

            var  now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);

            var addedschedule = mapper.Map<Schedule>(schedule);
            addedschedule.StartDate = TimeZoneInfo.ConvertTimeFromUtc(addedschedule.StartDate, egyptZone);
            addedschedule.EndDate = TimeZoneInfo.ConvertTimeFromUtc(addedschedule.EndDate, egyptZone);
            addedschedule.Status = now < addedschedule.StartDate ?ScheduleStatus.لم_تبدأ.ToString() :
                                   now > addedschedule.EndDate ? ScheduleStatus.اكتملت.ToString() :
                                                                      ScheduleStatus.في_تقدم.ToString();
            await _unitOfWork.Schedule.AddAsync(addedschedule);
            
            await _unitOfWork.CompleteAsync();
            return mapper.Map<ScheduleDisplayDTO>(addedschedule);

        }

        public async Task UpdateTheStatus(int Id,string Status)
        {
            var schdeule = await _unitOfWork.Schedule.GetByIdAsync(Id);
            if (schdeule is null)
                throw new Exception($"No Schedules with Id {Id}");
            schdeule.Status =Status;
            _unitOfWork.Schedule.Update(schdeule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int Id)
        {
            var schdeule=await _unitOfWork.Schedule.GetByIdAsync(Id);
            if (schdeule is null)
                throw new Exception($"No Schedules with Id {Id}");
           await _unitOfWork.Schedule.DeleteAsync(schdeule);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<ScheduleDisplayDTO> Get(int Id)
        {
            var schedule = await _unitOfWork.Schedule.GetByIdAsync(Id);
            if (schedule is null)
                throw new Exception($"No Schedules with Id {Id}");
            return mapper.Map<ScheduleDisplayDTO>(schedule);
        }

        public async Task<IEnumerable<ScheduleDisplayDTO>> GetAll(int CycleId)
        {

            var shedules = await _unitOfWork.Schedule.FindAllAsync(x => x.CycleId == CycleId);
            return mapper.Map<IEnumerable<ScheduleDisplayDTO>>(shedules);
        }

        public async Task<ScheduleDisplayDTO> update(UpdateScheduleDTO schedule)
        {
            var Existingschdeule = await _unitOfWork.Schedule.GetByIdAsync(schedule.ScheduleId);
            if (Existingschdeule is null)
                throw new Exception($"No Schedules with Id {schedule.ScheduleId}");
            if (schedule.StartDate > schedule.EndDate) throw new Exception("لا يمكن ان يكون موعد بدايه المهمه اكبر من موعد نهايه المهمه");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);


            schedule.StartDate = TimeZoneInfo.ConvertTimeFromUtc(schedule.StartDate, egyptZone);
            schedule.EndDate = TimeZoneInfo.ConvertTimeFromUtc(schedule.EndDate, egyptZone);

            if (Existingschdeule.Status == ScheduleStatus.في_تقدم.ToString() || Existingschdeule.Status == ScheduleStatus.اكتملت.ToString())
            {
                if (Existingschdeule.StartDate != schedule.StartDate)
                    throw new Exception("لا يمكن تعديل تاريخ البداية بعد بدء المهمة.");

                if (Existingschdeule.Status == ScheduleStatus.اكتملت.ToString() &&
                    Existingschdeule.EndDate != schedule.EndDate)
                    throw new Exception("لا يمكن تعديل تاريخ النهاية بعد اكتمال المهمة.");
            }


            mapper.Map(schedule, Existingschdeule);

            Existingschdeule.Status = now < Existingschdeule.StartDate ? ScheduleStatus.لم_تبدأ.ToString() :
                                   now > Existingschdeule.EndDate ? ScheduleStatus.اكتملت.ToString() :
                                                                      ScheduleStatus.في_تقدم.ToString();

            _unitOfWork.Schedule.Update(Existingschdeule);
            await _unitOfWork.CompleteAsync();
            return mapper.Map<ScheduleDisplayDTO>(Existingschdeule);
        }
        private async Task<Schedule> Later (Schedule schedule,string PropertyName,DateTime timelater)
        {
            if (schedule is null) throw new Exception("There is no Schdeules");
            if(PropertyName == "StartDate")
            {
                schedule.StartDate = schedule.StartDate.Add(timelater - schedule.StartDate);
                schedule.Status = "Pending";
            }
            else
            {
                schedule.EndDate = schedule.EndDate.Add(timelater - schedule.EndDate);
                schedule.Status = "InProgress";

            }
            return schedule;
        }
    }
}
