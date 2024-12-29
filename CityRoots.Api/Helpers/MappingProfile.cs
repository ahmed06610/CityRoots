using AutoMapper;
using CityRoots.Core.DTOs.Auth;
using CityRoots.Core.DTOs.Crop;

using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.CycleUpdate;

using CityRoots.Core.DTOs.Farm;
using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.DTOs.FeedBack;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.DTOs.OpenInvestmentCycle;
using CityRoots.Core.DTOs.Purchasereque;
using CityRoots.Core.DTOs.Schedule;
using CityRoots.Core.Models;

namespace CityRoots.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
                    .ForMember(dst => dst.PhoneNumberConfirmed, opt => opt.Ignore())
                    .ForMember(dst => dst.EmailConfirmed, opt => opt.Ignore())
                    .ForMember(dst => dst.AccessFailedCount, opt => opt.Ignore())
                    .ForMember(dst => dst.ConcurrencyStamp, opt => opt.Ignore())
                    .ForMember(dst => dst.Id, opt => opt.Ignore())
                    .ForMember(dst => dst.LockoutEnabled, opt => opt.Ignore())
                    .ForMember(dst => dst.LockoutEnd, opt => opt.Ignore())
                    .ForMember(dst => dst.NormalizedEmail, opt => opt.Ignore())
                    .ForMember(dst => dst.NormalizedUserName, opt => opt.Ignore())
                    .ForMember(dst => dst.SecurityStamp, opt => opt.Ignore())
                    .ForMember(dst => dst.TwoFactorEnabled, opt => opt.Ignore())
                    .ForSourceMember(src => src.Role, opt => opt.DoNotValidate());
            CreateMap<FeedBackRequest, FeedBack>();
            CreateMap<FeedBack,FeedBackDisplay>().
                ForMember(dst=>dst.UserName,opt=>opt.MapFrom(src=>src.User.Name));
            CreateMap<CreateFarmDTO, Farm>();
            CreateMap<UpdateFarmDTO, Farm>();
            CreateMap<Farm, FarmDTO>()
                .ForMember(dest => dest.LandParcels, opt => opt.MapFrom(src => src.LandParcels)); // Map nested objects

            CreateMap<CreateLandParcelDTO, LandParcel>();
            CreateMap<UpdateLandParcelDTO, LandParcel>();
            CreateMap<LandParcel, LandParcelDTO>()
     .ForMember(dest => dest.FarmLocation, opt => opt.MapFrom(src => src.Farm.Location))
     .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
         src.Cycles != null && src.Cycles.Any(cycle => cycle.StartDate <= DateTime.Now && cycle.EndDate >= DateTime.Now)
             ? "Occupied"
             : "Available"));
            CreateMap<Crop, CropDisplayDto>()
                .ForMember(dest=>dest.cropType,opt=>opt.MapFrom(src=>src.CropType.Name));
            CreateMap<AddCropDto, Crop>().ReverseMap();
            CreateMap<UpdateCropDto, Crop>().ReverseMap();
            CreateMap<Harvest, HarvestDisplayDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Crop.Name))
                .ForMember(dest => dest.FarmerDetails, opt => opt.MapFrom(src => src.Farmer))
                .ForMember(dest=>dest.CycleDetails,opt=>opt.MapFrom(x=>x.Cycle));
            CreateMap<AddHarvestDto,Harvest>().ReverseMap();
            CreateMap<UpdateHarvestDto,Harvest>().ReverseMap();
            CreateMap<Harvest, HarvestDtoForFarmer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Crop.Name))
                 .ForMember(dest => dest.ReuestsCount, opt => opt.MapFrom(src => src.Purchases.Count()))
;
            CreateMap<Cycle, CycleDetails>()
                .ForMember(dest => dest.LandImagesUrl, opt => opt.MapFrom(src => src.LandParcel.ImageUrl))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.LandParcel.Farm.Location))
                .ForMember(dest => dest.CycleUpdates, opt => opt.MapFrom(src => src.CycleUpdates));
            CreateMap<Farmer, FarmerDetails>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ApplicationUser.Name));
            CreateMap<CycleUpdate, CycleUpdatesForHarvestDto>();




            CreateMap<CreateCycleDTO, Cycle>();
            CreateMap<UpdateCycleDTO, Cycle>();
            CreateMap<Cycle, CycleDTO>();
            CreateMap<OpenInvestmentCycle, OpenInvestmentCycleDTO>();

            CreateMap<CreateOpenInvestmentCycleDTO, OpenInvestmentCycle>();
            CreateMap<UpdateOpenInvestmentCycleDTO, OpenInvestmentCycle>();
            CreateMap<AddScheduleDto, Schedule>();
            CreateMap<UpdateScheduleDTO, Schedule>().ReverseMap();
            CreateMap<Schedule, ScheduleDisplayDTO>()
                .ForMember(dest => dest.StartDate,
                           opt => opt.MapFrom(src => src.StartDate.ToString("dd/MM/yyyy HH:mm")))
                .ForMember(dest => dest.EndDate,
                           opt => opt.MapFrom(src => src.EndDate.ToString("dd/MM/yyyy HH:mm")));

            CreateMap<CreateCycleUpdateDTO, CycleUpdate>();
            CreateMap<UpdateCycleUpdateDTO, CycleUpdate>();
            CreateMap<CycleUpdate, CycleUpdateDTO>();
            CreateMap<PurchaseRequest, AllPurchasesRequestForHarvest>()
           .ForMember(dest => dest.merchantName, opt => opt.MapFrom(src => src.Merchant.ApplicationUser.Name));
            
            
           



        }
    }
}
