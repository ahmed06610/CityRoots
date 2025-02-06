using AutoMapper;
using CityRoots.Core.DTOs.Rate;
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
    public class RateService:IRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RateService(IUnitOfWork unitOfWork,IMapper mapper) { 
        _unitOfWork = unitOfWork;
            _mapper = mapper;
        
        }

        public async Task DeleteTheRating(DeleteRate rate)
        {
            var Deletedrate=await _unitOfWork.Rate.FindTWithExpression<Rate>(x=>x.UserId==rate.UserId && x.FarmerId==rate.FarmerId);
            if (rate is null)
                throw new Exception($"You didnot rate this farmer before {rate.FarmerId}");
            await _unitOfWork.Rate.DeleteAsync(Deletedrate);
            await _unitOfWork.CompleteAsync();
        }

        public async Task MakeTheRating(RateRequest rate)
        {
            await _unitOfWork.Rate.AddAsync(_mapper.Map<Rate>(rate));
            await _unitOfWork.CompleteAsync();
        }
    }
}
