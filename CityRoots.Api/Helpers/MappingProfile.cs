using AutoMapper;
using CityRoots.Core.DTOs.Auth;
using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Farm;
using CityRoots.Core.DTOs.FeedBack;
using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.DTOs.OpenInvestmentCycle;
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
                .ForMember(dest => dest.FarmLocation, opt => opt.MapFrom(src => src.Farm.Location)); // Map Farm's Location

            CreateMap<CreateCycleDTO, Cycle>();
            CreateMap<UpdateCycleDTO, Cycle>();
            CreateMap<Cycle, CycleDTO>();
            CreateMap<OpenInvestmentCycle, OpenInvestmentCycleDTO>();

            CreateMap<CreateOpenInvestmentCycleDTO, OpenInvestmentCycle>();
            CreateMap<UpdateOpenInvestmentCycleDTO, OpenInvestmentCycle>();



        }
    }
}
