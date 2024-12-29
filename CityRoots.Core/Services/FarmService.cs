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
using CityRoots.Core.DTOs.LandParcel;

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

        public async Task<List<FarmDTO>> GetAllFarmsAsync(int FarmerId=0)
        {
             var Farms= (await _unitOfWork.Farm.FindAllWithIncludes<Farm>(f=>f.FarmerId==FarmerId || FarmerId==0,
                f=>f.LandParcels)).ToList();
            var farms = new List<FarmDTO>();
            foreach (var farm in Farms)
            {
               var f= await mapping(farm);
                farms.Add(f);
            }
            return farms;
        }

        public async Task<FarmDTO> GetFarmByIdAsync(int id)
        {
            var farm = await _unitOfWork.Farm.FindTWithIncludes<Farm>(id, "FarmId", f => f.LandParcels);
            return await mapping(farm);
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
        private async Task<FarmDTO> mapping(Farm farm)
        {
            var farmLands = new List<LandParcel>();
            foreach (var land in farm.LandParcels)
            {
                var lands = await _unitOfWork.LandParcel.FindTWithIncludes<LandParcel>(land.ParcelId, "ParcelId", l => l.Cycles);
                farmLands.Add(land);
            }
            farm.LandParcels= farmLands;
            var farmDto= _mapper.Map<FarmDTO>(farm);
            farmDto.numbersOfLands=farmLands.Count();
            return farmDto;
        }
    }
}
