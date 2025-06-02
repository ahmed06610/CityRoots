using AutoMapper;
using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class FarmerService : IFarmerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper mapper;

        public FarmerService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<FarmerInfoDTO> GetFarmerInfo(int id)
        {
            var farmer = await _unitOfWork.Farmer.GetByIdAsync(id);
            if (farmer == null)
                return null;
            var user = await _userManager.FindByIdAsync(farmer.ApplicationUserId);
            var ratings = (await _unitOfWork.Rate.FindAllWithIncludes<Rate>(r => r.FarmerId == farmer.ApplicationUserId));

            var info = new FarmerInfoDTO
            {
                FarmerId = farmer.FarmerId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Rate = ratings.Count() != 0 ? (int)ratings.Average(r => r.Rating) : 0,
                Bio = farmer.Bio,
                ImageUrl=user.ImageProfileUrl,
                UserId=user.Id
            };
            return info;
        }

        public async Task<FarmerInfoDTO> UpdateFarmer(UpdateFarmerDTO model)
        {
            var updated = await _userManager.FindByIdAsync(_unitOfWork.Farmer.GetByIdAsync(model.FarmerId).Result.ApplicationUserId);
            updated.PhoneNumber = model.Phone;
            updated.Name = model.FarmerName;
            await _userManager.UpdateAsync(updated);
            var farmerup = await _unitOfWork.Farmer.GetByIdAsync(model.FarmerId);
            farmerup.Bio = model.Bio;
            _unitOfWork.Farmer.Update(farmerup);
            await _unitOfWork.CompleteAsync();

            var infoUpdated = new FarmerInfoDTO { Name = updated.Name, Phone = updated.PhoneNumber, Email = updated.Email, FarmerId = model.FarmerId, Bio = model.Bio };
            return infoUpdated;
        }
    }
}
