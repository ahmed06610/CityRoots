using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityRoots.Core.DTOs.Farm;
using AutoMapper;

namespace CityRoots.Core.Services
{
    public class FarmService : IFarmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public FarmService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Farm>> GetAllFarmsAsync(int FarmerId=0)
        {
            return (await _unitOfWork.Farm.FindAllAsync(f=>f.FarmerId==FarmerId || FarmerId==0)).ToList();
        }

        public async Task<Farm> GetFarmByIdAsync(int id)
        {
            return await _unitOfWork.Farm.GetByIdAsync(id);
        }

        public async Task<FarmDTO> AddFarmAsync(CreateFarmDTO createFarmDto)
        {
            var farm = _mapper.Map<Farm>(createFarmDto);
            var createdFarm = await _unitOfWork.Farm.AddAsync(farm);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<FarmDTO>(createdFarm);
        }

        public async Task<FarmDTO> UpdateFarmAsync(UpdateFarmDTO updateFarmDto)
        {
            var existingFarm = await _unitOfWork.Farm.GetByIdAsync(updateFarmDto.FarmId);
            if (existingFarm == null) return null;

            _mapper.Map(updateFarmDto, existingFarm);
            _unitOfWork.Farm.Update(existingFarm);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<FarmDTO>(existingFarm);
        }

        public async Task<bool> DeleteFarmAsync(int id)
        {
            var farm = await _unitOfWork.Farm.GetByIdAsync(id);
            if (farm == null) return false;

            await _unitOfWork.Farm.DeleteAsync(farm);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
