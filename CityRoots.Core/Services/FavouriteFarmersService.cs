using AutoMapper;
using CityRoots.Core.DTOs.FavouriteFarmers;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tensorflow.Keras.Layers;

namespace CityRoots.Core.Services
{
    public class FavouriteFarmersService : IFavouriteFarmersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FavouriteFarmersService(IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task AddToFavourites(string FarmerId,string? userId)
        {
            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId is null)
            //{
            //    throw new Exception("User ID not found in token");

            //}
            var favoriteFarmer = new FavoriteFarmers
            {
                FarmerId = FarmerId,
                userId = userId
            };
            await _unitOfWork.FavoriteFarmers.AddAsync(favoriteFarmer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<FavouriteFarmerDTO>> GetAllFavourites(string? userId)
        {
           // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           //// userId = "afe5a2cf-4b8a-4dc7-ac6d-025698ded2d2";
           // if (userId is null) {
           //     throw new Exception("User ID not found in token");
            
           // }
            var farmers=(await _unitOfWork.FavoriteFarmers.FindAllWithIncludes<FavoriteFarmers>(
                x=>x.userId==userId,
                x=>x.FarmerUser,
                x=>x.FarmerUser.Farmer)).ToList();
            return _mapper.Map<List<FavouriteFarmerDTO>>(farmers);
        }

      
        public async Task RemoveFromFavourites(string FarmerId,string? userId)
        {
            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId is null)
            //{
            //    throw new Exception("User ID not found in token");

            //}
            var favoriteFarmer = new FavoriteFarmers
            {
                FarmerId = FarmerId,
                userId = userId
            };
            await _unitOfWork.FavoriteFarmers.DeleteAsync(favoriteFarmer);
            await _unitOfWork.CompleteAsync();
        }
    }
}
