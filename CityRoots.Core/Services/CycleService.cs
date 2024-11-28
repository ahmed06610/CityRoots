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
                c => c.LandParcel.Farm.Farmer)).ToList();
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
            return cycleDto;
        }
    }

}
