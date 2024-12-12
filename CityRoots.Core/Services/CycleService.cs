using AutoMapper;
using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class CycleService : ICycleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CycleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CycleDTO>> GetAllCyclesAsync(int FarmerId = 0)
        {
            var cycles = (await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(null,
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.InvestmentRequests)).ToList();
            cycles = cycles.Where(c => c.LandParcel.Farm.FarmerId == FarmerId || FarmerId == 0).ToList();
            var cyclesDTOs = new List<CycleDTO>();

            foreach (var cycle in cycles)
            {
                var cycleDto = await mapping(cycle);
                cyclesDTOs.Add(cycleDto);
            }
            return cyclesDTOs;
        }

        public async Task<CycleDTO> GetCycleByIdAsync(int id)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(id);
            return await mapping(cycle);

        }

        public async Task<CycleDTO> AddCycleAsync(CreateCycleDTO createCycleDto)
        {
            var cycle = _mapper.Map<Cycle>(createCycleDto);
            var createdCycle = await _unitOfWork.Cycle.AddAsync(cycle);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CycleDTO>(createdCycle);
        }

        public async Task<CycleDTO> UpdateCycleAsync(UpdateCycleDTO updateCycleDto)
        {
            var existingCycle = await _unitOfWork.Cycle.GetByIdAsync(updateCycleDto.CycleId);
            if (existingCycle == null) return null;

            _mapper.Map(updateCycleDto, existingCycle);
            _unitOfWork.Cycle.Update(existingCycle);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CycleDTO>(existingCycle);
        }

        public async Task<bool> DeleteCycleAsync(int id)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(id);
            if (cycle == null) return false;

            await _unitOfWork.Cycle.DeleteAsync(cycle);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        private async Task<CycleDTO> mapping(Cycle cycle)
        {
            var cycleDto = _mapper.Map<CycleDTO>(cycle);
            var openCycle = await _unitOfWork.OpenInvestmentCycle.FindTWithExpression<OpenInvestmentCycle>(oc => oc.CycleId == cycle.CycleId);
            if (openCycle == null)
            {
                cycleDto.IsOpenForInvestment = false;
            }
            else
            {
                cycleDto.IsOpenForInvestment = true;
                var openCycleDto = _mapper.Map<OpenInvestmentCycleDTO>(openCycle);
                cycleDto.OpenInvestmentCycleDTO = openCycleDto;
            }
            if (cycle.InvestmentRequests != null)
            {
                var CurrentInvestors = new List<CurrentInvestors>();
                foreach (var investment in cycle.InvestmentRequests)
                {
                    var invest =await _unitOfWork.Investor.FindTWithIncludes<Investor>(investment.InvestorId, "InvestorId",
                        i => i.ApplicationUser);
                    var CurrentInvstment = new CurrentInvestors
                    {
                        FullName = invest.ApplicationUser.UserName,
                        InvestmentAmount = investment.RequestedAmount,
                    };
                    CurrentInvestors.Add(CurrentInvstment);
                }
                cycleDto.currentInvestors = CurrentInvestors;
            }

            cycleDto.TimeToStart = GetRemainingTimeMessage(cycleDto.StartDate,cycleDto.EndDate);

            return cycleDto;
        }
        private string GetRemainingTimeMessage(DateTime startDate,DateTime endDate)
        {
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            // Get the current time in Egypt
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, egyptTimeZone);
            if (endDate <= now)
            {
                return "الدورة قد انتهت بالفعل";
            }
            if (startDate <= now)
            {
                return "الدورة قد بدأت بالفعل";
            }

            var remainingTime = startDate - now;
            var days = remainingTime.Days;
            var weeks = days / 7;
            var months = days / 30;

            string message;

            if (months > 0)
            {
                message = $"متبقي فقط {months} شهر{(months > 1 ? "اً" : "")} على بداية الدورة";
            }
            else if (weeks > 0)
            {
                message = $"متبقي فقط {weeks} أسبوع{(weeks > 1 ? "ين" : "")} على بداية الدورة";
            }
            else
            {
                message = $"متبقي فقط {days} يوم{(days > 1 ? "اً" : "")} على بداية الدورة";
            }

            return message;
        }
    }



}
