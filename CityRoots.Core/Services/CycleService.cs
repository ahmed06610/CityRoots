using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Cycle;
using CityRoots.Core.DTOs.Recommendation;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tensorflow;

namespace CityRoots.Core.Services
{
    public class CycleService : ICycleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOpenInvestmentCycleService _openInvestmentCycleService;
        private readonly IPaymentService _paymentService;
        private readonly IFarmerService _farmerService;
        private readonly ILandParcelService _landParcel;
        private readonly IOpenInvestmentCycleService _investmentCycleService;
        private readonly ICycleUpdateService _cycleUpdateService;

        public CycleService(IUnitOfWork unitOfWork, IMapper mapper, IOpenInvestmentCycleService openInvestmentCycleService,
            IPaymentService paymentService, IFarmerService farmerService, ILandParcelService landParcel,
            IOpenInvestmentCycleService investmentCycleService, ICycleUpdateService cycleUpdateService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _openInvestmentCycleService = openInvestmentCycleService;
            _paymentService = paymentService;
            _farmerService = farmerService;
            _landParcel = landParcel;
            _investmentCycleService = investmentCycleService;
            _cycleUpdateService = cycleUpdateService;
        }

        public async Task<List<CycleForFarmerDTO>> GetAllCyclesForFarmersAsync(int FarmerId = 0,bool f=true)
        {
            var cycles = (await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(null,
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.InvestmentRequests)).ToList();
            cycles = cycles.Where(c => c.LandParcel.Farm.FarmerId == FarmerId || FarmerId == 0).ToList();
            var cyclesDTOs = new List<CycleForFarmerDTO>();

            foreach (var cycle in cycles)
            {
                var cycleDto = await mapping(cycle,f);
                cyclesDTOs.Add(cycleDto);
            }
            return cyclesDTOs;
        }
        public async Task<List<CycleDTO>> GetAllCyclesForInvestorsAsync(InvestorRecommendationResponseDTO Recommendation =null)
        {
            Expression<Func<Cycle, bool>> criteria = c =>
        (Recommendation != null && Recommendation.recommended_cycle_ids.Contains(c.CycleId)) ||
        (Recommendation == null && c.OpenInvestmentCycle != null);
            var cycles = (await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(criteria,
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.OpenInvestmentCycle 
              )).ToList();
            var cyclesDTOs = new List<CycleDTO>();

            foreach (var cycle in cycles)
            {
                var cycleDto = await mapping(cycle, false);
                cyclesDTOs.Add(cycleDto);
            }
            return cyclesDTOs;
        }
        public async Task<CycleForFarmerDTO> GetCycleByIdAsync(int id)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(id);
            return await mapping(cycle);

        }
        public async Task<CycleForInvestorDTO> GetCycleByIdForInvestorAsync(int Cycleid,int InvestorId)
        {
            var cycleForInvestor = new CycleForInvestorDTO();
            var cycle=await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(Cycleid, "CycleId",
                c=>c.LandParcel,
                c=>c.LandParcel.Farm,   
                c=>c.LandParcel.Farm.Farmer,
                 c => c.InvestmentRequests
                );
            var cycleName=cycle.CycleName;
            var farmerInfo = await _farmerService.GetFarmerInfo(cycle.LandParcel.Farm.FarmerId);
            var landParcel = await _landParcel.GetLandParcelByIdAsync(cycle.LandParcel.ParcelId);
            var investmentCycle=(await mapping(cycle,false));
            var x = true ? (await _unitOfWork.InvestmentRequest.FindTWithExpression<InvestmentRequest>(ir => ir.InvestorId == InvestorId && ir.CycleId == Cycleid && ir.RequestStatus == InvestmentStatues.Accepted.ToString())) != null : false;
            if (x == true)
            {
                cycleForInvestor.IsInvestorSub=true;
                var cycleUpdates = await _cycleUpdateService.GetAllUpdatesByCycleIdAsync(Cycleid);
                cycleForInvestor.cycleUpdates = cycleUpdates.ToList();
            }
            cycleForInvestor.RequestReview = true ? (await _unitOfWork.InvestmentRequest.FindTWithExpression<InvestmentRequest>
                (ir => ir.InvestorId==InvestorId && ir.CycleId == Cycleid
               && ir.RequestStatus == InvestmentStatues.Pending.ToString()) is not null) : false;
            cycleForInvestor.InvestmentCycle = investmentCycle;
            cycleForInvestor.Farmer = farmerInfo;
            cycleForInvestor.landParcel= landParcel;
            cycleForInvestor.CycleName = cycleName;
            return cycleForInvestor;
        }
        public async Task<CycleDTO> AddCycleAsync(CreateCycleDTO createCycleDto)
        {
            var cycle = _mapper.Map<Cycle>(createCycleDto);
            var createdCycle = await _unitOfWork.Cycle.AddAsync(cycle);
            await _unitOfWork.CompleteAsync();

            if (createCycleDto.openInvestmentCycleDTO != null)
            {
                createCycleDto.openInvestmentCycleDTO.CycleId = createdCycle.CycleId;
              
                    await _openInvestmentCycleService.CreateOpenInvestmentCycleAsync(createCycleDto.openInvestmentCycleDTO);

              
            }
            await _unitOfWork.CompleteAsync();
            var result =await mapping(createdCycle);

            return result;
        }
        public async Task<CycleDTO> UpdateCycleAsync(UpdateCycleDTO updateCycleDto)
        {
            var existingCycle = await _unitOfWork.Cycle.GetByIdAsync(updateCycleDto.CycleId);
            if (existingCycle == null) return null;


            _mapper.Map(updateCycleDto, existingCycle);
            _unitOfWork.Cycle.Update(existingCycle);
            if (updateCycleDto.UpdateOpenInvestmentCycleDTO != null)
            {
                updateCycleDto.UpdateOpenInvestmentCycleDTO.CycleId= existingCycle.CycleId;
                updateCycleDto.UpdateOpenInvestmentCycleDTO.OpenInvestmentCycleId=(await _unitOfWork.OpenInvestmentCycle
                    .FindTWithExpression<OpenInvestmentCycle>(o=>o.CycleId==existingCycle.CycleId)).OpenInvestmentCycleId;
               await _openInvestmentCycleService.UpdateOpenInvestmentCycleAsync(updateCycleDto.UpdateOpenInvestmentCycleDTO);
            }
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CycleDTO>(existingCycle);
        }
        public async Task<bool> DeleteCycleAsync(int id)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(id);
            if (cycle == null) return false;
            var openInvestmentCycle = await _unitOfWork.OpenInvestmentCycle.FindTWithExpression<OpenInvestmentCycle>(o => o.CycleId == id);
            if (openInvestmentCycle != null)
            {
                await _openInvestmentCycleService.DeleteOpenInvestmentCycleAsync(openInvestmentCycle.OpenInvestmentCycleId);
            }
           await _paymentService.DeletePaymentsByCycleIdAsync(id);
            await _unitOfWork.Cycle.DeleteAsync(cycle);
            
            await _unitOfWork.CompleteAsync();
            return true;
        }
        private async Task<CycleForFarmerDTO> mapping(Cycle cycle,bool f=true)
        {
            var cycleDto = _mapper.Map<CycleForFarmerDTO>(cycle);
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
            if (cycle.InvestmentRequests != null&&f==true)
            {
                var CurrentInvestors = new List<CurrentInvestors>();
                var ReqestForInvestment = new List<RequestsForInvestment>();
                foreach (var investment in cycle.InvestmentRequests)
                {
                    cycleDto.NumbersOfRequestsInvestments = 0;
                    if (investment.RequestStatus == InvestmentStatues.Accepted.ToString()) {
                        var invest = await _unitOfWork.Investor.FindTWithIncludes<Investor>(investment.InvestorId, "InvestorId",
                            i => i.ApplicationUser);
                        var CurrentInvstment = new CurrentInvestors
                        {
                            FullName = invest.ApplicationUser.UserName,
                            InvestmentAmount = investment.RequestedAmount,
                        };
                        CurrentInvestors.Add(CurrentInvstment);
                    }
                    else if( investment.RequestStatus == InvestmentStatues.Pending.ToString())
                    {
                        cycleDto.NumbersOfRequestsInvestments += 1;

                        var invest = await _unitOfWork.Investor.FindTWithIncludes<Investor>(investment.InvestorId, "InvestorId",
                           i => i.ApplicationUser);
                        var requestForInvestment = new RequestsForInvestment
                        {
                            FullName = invest.ApplicationUser.UserName,
                            InvestmentAmount = investment.RequestedAmount,
                            TypeOfProfit = investment.RequestedProfitType.ToString(),
                        };
                        ReqestForInvestment.Add(requestForInvestment);

                    }
                    cycleDto.requestsForInvestments = ReqestForInvestment;
                    cycleDto.currentInvestors = CurrentInvestors;
                }
            }

            cycleDto.TimeToStart = GetRemainingTimeMessage(cycleDto.StartDate,cycleDto.EndDate);

            return cycleDto;
        }
        private string GetRemainingTimeMessage(DateTime startDate,DateTime endDate)
        {
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            // Get the current time in Egypt
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, egyptTimeZone);
            if (endDate <= now)
            {
                return "الدورة قد انتهت بالفعل";
            }
            if (startDate <= now)
            {
                return "الدورة قد بدأت بالفعل";
            }

            var remainingTime = startDate - now;
            var days = remainingTime.Days;
            var weeks = days / 7;
            var months = days / 30;

            string message;

            if (months > 0)
            {
                message = $"متبقي فقط {months} شهر{(months > 1 ? "اً" : "")} على بداية الدورة";
            }
            else if (weeks > 0)
            {
                message = $"متبقي فقط {weeks} أسبوع{(weeks > 1 ? "ين" : "")} على بداية الدورة";
            }
            else
            {
                message = $"متبقي فقط {days} يوم{(days > 1 ? "اً" : "")} على بداية الدورة";
            }

            return message;
        }
    }



}
