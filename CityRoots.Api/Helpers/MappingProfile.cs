using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Auth;
using CityRoots.Core.DTOs.Crop;

using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.CycleUpdate;

using CityRoots.Core.DTOs.Farm;
using CityRoots.Core.DTOs.Farmer;
using CityRoots.Core.DTOs.FavouriteFarmers;
using CityRoots.Core.DTOs.FeedBack;
using CityRoots.Core.DTOs.Harvest;
using CityRoots.Core.DTOs.InvestmentRequests;
using CityRoots.Core.DTOs.LandParcel;
using CityRoots.Core.DTOs.OpenInvestmentCycle;
using CityRoots.Core.DTOs.Payment;
using CityRoots.Core.DTOs.Purchasereque;
using CityRoots.Core.DTOs.Purchaserequest;
using CityRoots.Core.DTOs.Rate;
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
         src.Cycles != null && src.Cycles.Any(cycle => cycle.StartDate <= TimeHelper.NowInEgypt && cycle.EndDate >= TimeHelper.NowInEgypt)
             ? "مربوطه بدوره زراعيه"
             : "متاحه"))
     .ForMember(dest => dest.CycleName, opt => opt.MapFrom(src =>
         src.Cycles != null && src.Cycles.Any(cycle => cycle.StartDate <= TimeHelper.NowInEgypt && cycle.EndDate >= TimeHelper.NowInEgypt)
             ? src.Cycles.FirstOrDefault(cycle => cycle.StartDate <= TimeHelper.NowInEgypt && cycle.EndDate >= TimeHelper.NowInEgypt).CycleName
             : null));
            CreateMap<Crop, CropDisplayDto>()
                .ForMember(dest=>dest.cropType,opt=>opt.MapFrom(src=>src.CropType.Name));
            CreateMap<AddCropDto, Crop>().ReverseMap();
            CreateMap<UpdateCropDto, Crop>().ReverseMap();
            CreateMap<Crop, CropDTO>()
            .ForMember(dest => dest.CropName, opt => opt.MapFrom(src => src.Name));


            CreateMap<Harvest, HarvestDisplayDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Crop.Name))
                .ForMember(dest => dest.FarmerDetails, opt => opt.MapFrom(src => src.Farmer))
                .ForMember(dest=>dest.CycleDetails,opt=>opt.MapFrom(x=>x.Cycle));
            CreateMap<AddHarvestDto,Harvest>().ReverseMap();
            CreateMap<UpdateHarvestDto,Harvest>().ReverseMap();
            CreateMap<Harvest, HarvestDtoForFarmer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Crop.Name))
                 .ForMember(dest => dest.ReuestsCount, opt => opt.MapFrom(src => src.Purchases.Count()));
            CreateMap<Harvest, HarvestForBrowsing>()
                .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.Yield))
                .ForMember(dest => dest.PricePerUnit, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.HarvestDate, opt => opt.MapFrom(src => src.ProductionDate))
                .ForMember(dest => dest.CropType, opt => opt.MapFrom(src => src.Crop.CropType.Name))
                .ForMember(dest => dest.FarmLocation, opt => opt.MapFrom(src => src.Cycle.LandParcel.Farm.Location));
            CreateMap<Harvest, HarvestDetailsDTO>()
                .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => src.Yield))
                .ForMember(dest => dest.HarvestStatus, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.HarvestDate, opt => opt.MapFrom(src => src.ProductionDate))
                .ForMember(dest => dest.CropType, opt => opt.MapFrom(src => src.Crop.CropType.Name))
                .ForMember(dest => dest.CropName, opt => opt.MapFrom(src => src.Crop.Name));




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
            CreateMap<Cycle, CycleDTO>().ForMember(dest => dest.CropName, opt => opt.MapFrom(src => src.Crop.Name))
                .ForMember(dest => dest.ParcelName, opt => opt.MapFrom(src => src.LandParcel.ParcelName)).ReverseMap();
            CreateMap<Cycle, CycleForFarmerDTO>()
                .ForMember(dest=>dest.CropName,opt=>opt.MapFrom(src=>src.Crop.Name))
                .ForMember(dest=>dest.ParcelName,opt=>opt.MapFrom(src=>src.LandParcel.ParcelName));
            CreateMap<OpenInvestmentCycle, OpenInvestmentCycleDTO>();

            CreateMap<CreateOpenInvestmentCycleDTO, OpenInvestmentCycle>();
            CreateMap<UpdateOpenInvestmentCycleDTO, OpenInvestmentCycle>().ReverseMap();
            CreateMap<UpdateOpenInvestmentCycleDTO, OpenInvestmentCycleDTO>().ReverseMap();
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
            CreateMap<Cycle, CycleForInvestorDTO>();
            CreateMap<Cycle, CycleForBrowsing>()
                .ForMember(dest => dest.IsOpenForInvestment, opt => opt.MapFrom(src => src.OpenInvestmentCycle != null))
                .ForMember(dest => dest.OpenInvestmentCycleDTO, opt => opt.MapFrom(src => src.OpenInvestmentCycle))
                .ForMember(dest => dest.ParcelName, opt => opt.MapFrom(src => src.LandParcel.ParcelName));




            CreateMap<PurchaseRequest, AllPurchasesRequestForHarvest>()
           .ForMember(dest => dest.merchantName, opt => opt.MapFrom(src => src.Merchant.ApplicationUser.Name))
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Merchant.ApplicationUser.Id))
           .ForMember(dest => dest.UserImageUrl, opt => opt.MapFrom(src => src.Merchant.ApplicationUser.ImageProfileUrl))
           ;
            CreateMap<FavoriteFarmers, FavouriteFarmerDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FarmerUser.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FarmerUser.Name))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.FarmerUser.PhoneNumber))
                .ForMember(dest => dest.bio, opt => opt.MapFrom(src => src.FarmerUser.Farmer.Bio))
                .ForMember(dest=>dest.FarmerId,opt=>opt.MapFrom(src=>src.FarmerId))
                .ForMember(dest=>dest.ImageProfileUrl,opt=>opt.MapFrom(src=>src.FarmerUser.ImageProfileUrl));
           
            ;
            CreateMap<InvestmentRequest, InvestmentrequestDisplay>()
                .ForMember(dest => dest.cycleName, opt => opt.MapFrom(src => src.Cycle.CycleName))
                .ForMember(dest => dest.farmerName, opt => opt.MapFrom(src => src.Cycle.LandParcel.Farm.Farmer.ApplicationUser.Name));
                CreateMap<CreateInvestmentRequest, InvestmentRequest>();
            CreateMap<RateRequest, Rate>();
            CreateMap<PurchaseRequest, PurchaseRequestDsiplay>()
                .ForMember(dest => dest.HarvestName, opt => opt.MapFrom(src => src.Harvest.Crop.Name))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.Merchant.ApplicationUser.Name));
            CreateMap<CreatePurchaseRrquest, PurchaseRequest>();

            // Merchant Mappings
            CreateMap<Payment, MerchantPaymentDetailDto>()
                .ForMember(dest => dest.PayerName, opt => opt.MapFrom(src => src.Payer.Name))
                .ForMember(dest => dest.PayerEmail, opt => opt.MapFrom(src => src.Payer.Email))
                .ForMember(dest => dest.PayeeName, opt => opt.MapFrom(src => src.Payee.Name))
                .ForMember(dest => dest.PayeeEmail, opt => opt.MapFrom(src => src.Payee.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Statue))
                .ForMember(dest => dest.AssociatedHarvest, opt => opt.MapFrom(src => new HarvestDto
                {
                    HarvestId = src.Harvest.HarvestId,
                    CropName = src.Harvest.Crop.Name
                }));

            CreateMap<Payment, PaymentMerchantSummaryDto>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.PaymentDate.Year))
                .ForMember(dest => dest.PurchasesPerMonth, opt => opt.Ignore()); // Will be populated manually

            // Investor Mappings
            CreateMap<Payment, InvestorPaymentDetailDto>()
                .ForMember(dest => dest.PayeeName, opt => opt.MapFrom(src => src.Payee.Name))
                .ForMember(dest => dest.PayeeEmail, opt => opt.MapFrom(src => src.Payee.Email))
                .ForMember(dest => dest.PayerEmail, opt => opt.MapFrom(src => src.Payer.Email))
                .ForMember(dest => dest.PayerName, opt => opt.MapFrom(src => src.Payer.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Statue))
                .ForMember(dest => dest.AssociatedCycle, opt => opt.MapFrom(src => new CycleDto
                {
                    CycleId = src.Cycle.CycleId,
                    CycleName = src.Cycle.CycleName
                }));

            CreateMap<Payment, PaymentInvestorSummaryDto>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.PaymentDate.Year))
                .ForMember(dest => dest.InvestmentsPerMonth, opt => opt.Ignore()); // Will be populated manually








        }
    }
}
