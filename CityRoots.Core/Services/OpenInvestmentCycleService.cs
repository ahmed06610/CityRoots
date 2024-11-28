using CityRoots.Core.DTOs.OpenInvestmentCycle;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CityRoots.Core.DTOs.Cycle;

namespace CityRoots.Core.Services
{
    public class OpenInvestmentCycleService : IOpenInvestmentCycleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public OpenInvestmentCycleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OpenInvestmentCycleDTO> CreateOpenInvestmentCycleAsync(CreateOpenInvestmentCycleDTO dto)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(dto.CycleId);
            if (cycle == null) throw new KeyNotFoundException("Cycle not found.");

            var openInvestmentCycle = _mapper.Map<OpenInvestmentCycle>(dto);

            await _unitOfWork.OpenInvestmentCycle.AddAsync(openInvestmentCycle);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<OpenInvestmentCycleDTO>(openInvestmentCycle);
        }

        public async Task<OpenInvestmentCycleDTO> UpdateOpenInvestmentCycleAsync(UpdateOpenInvestmentCycleDTO dto)
        {
            var openInvestmentCycle = await _unitOfWork.OpenInvestmentCycle.GetByIdAsync(dto.OpenInvestmentCycleId);
            if (openInvestmentCycle == null) throw new KeyNotFoundException("OpenInvestmentCycle not found.");

            _mapper.Map(dto,openInvestmentCycle);

            _unitOfWork.OpenInvestmentCycle.Update(openInvestmentCycle);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<OpenInvestmentCycleDTO>(openInvestmentCycle);
        }

        public async Task<bool> DeleteOpenInvestmentCycleAsync(int id)
        {
            var openInvestmentCycle = await _unitOfWork.OpenInvestmentCycle.GetByIdAsync(id);
            if (openInvestmentCycle == null) throw new KeyNotFoundException("OpenInvestmentCycle not found.");

            await _unitOfWork.OpenInvestmentCycle.DeleteAsync(openInvestmentCycle);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
