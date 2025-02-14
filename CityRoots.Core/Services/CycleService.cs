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
        private readonly IInvestmentRequestService _investmentRequestService;

        public CycleService(IUnitOfWork unitOfWork, IMapper mapper, IOpenInvestmentCycleService openInvestmentCycleService,
            IPaymentService paymentService, IFarmerService farmerService, ILandParcelService landParcel,
            IOpenInvestmentCycleService investmentCycleService, ICycleUpdateService cycleUpdateService, IInvestmentRequestService investmentRequestService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _openInvestmentCycleService = openInvestmentCycleService;
            _paymentService = paymentService;
            _farmerService = farmerService;
            _landParcel = landParcel;
            _investmentCycleService = investmentCycleService;
            _cycleUpdateService = cycleUpdateService;
            _investmentRequestService = investmentRequestService;
        }

        public async Task<List<CycleForFarmerDTO>> GetAllCyclesForFarmersAsync(int FarmerId = 0,bool ForFarmer = true)
        {
            var cycles = (await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(null,
                c => c.LandParcel,
                c => c.Crop,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.InvestmentRequests)).ToList();
            cycles = cycles.Where(c => c.LandParcel.Farm.FarmerId == FarmerId || FarmerId == 0).ToList();
            var cyclesDTOs = new List<CycleForFarmerDTO>();

            foreach (var cycle in cycles)
            {
                var cycleDto = await mapping(cycle, ForFarmer);
                cyclesDTOs.Add(cycleDto);
            }
            return cyclesDTOs;
        }
        public async Task<List<CycleForBrowsing>> GetAllCyclesForInvestorsAsync(InvestorRecommendationResponseDTO Recommendation =null)
        {
            Expression<Func<Cycle, bool>> criteria = null;

            if (Recommendation != null && Recommendation.recommended_cycle_ids != null && Recommendation.recommended_cycle_ids.Any())
            {
                criteria = c => Recommendation.recommended_cycle_ids.Contains(c.CycleId);
            }
           
            else
            {
                // Get All Open Cycles
                criteria = c => c.OpenInvestmentCycle != null;
            }
            var cycles = (await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(criteria,
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.LandParcel.Farm.Farmer.ApplicationUser,
                c => c.OpenInvestmentCycle,
                c => c.InvestmentRequests,
                c => c.Crop

              )).ToList();
            var cyclesDTOs = new List<CycleForBrowsing>();

            foreach (var cycle in cycles)
            {
                var f= cycle.LandParcel.Farm.Farmer.ApplicationUser.Id;
                var x = (await _unitOfWork.Rate.FindAllWithIncludes<Rate>(r => r.FarmerId == f));
                var rate = x.Count() != 0 ? (int)x.Average(r => r.Rating) : 0;
                var cycleDto = _mapper.Map<CycleForBrowsing>(cycle);
                cycleDto.Rate =rate;
                cycleDto.TimeToStart = GetRemainingTimeMessage(cycleDto.StartDate, cycleDto.EndDate);
                cyclesDTOs.Add(cycleDto);
            }
            return cyclesDTOs;
        }
        public async Task<List<CycleForInvestorDTO>> GetAllPrivateCyclesForInvestor(int InvestorId)
        {
           

            var cycles = (await _unitOfWork.Cycle.FindAllWithIncludes<Cycle>(c => c.InvestmentRequests.Any(ir => ir.InvestorId == InvestorId && ir.RequestStatus == InvestmentStatues.مقبول.ToString()),
                c => c.LandParcel,
                c => c.LandParcel.Farm,
                c => c.OpenInvestmentCycle,
                c => c.InvestmentRequests,
                c => c.Crop

              )).ToList();
            var cyclesDTOs = new List<CycleForInvestorDTO>();

            foreach (var cycle in cycles)
            {
                var cycleDto = await mappingForInvestor(cycle,InvestorId);
                cyclesDTOs.Add(cycleDto);
            }
            return cyclesDTOs;


        }
        public async Task<CycleForFarmerDTO> GetCycleByIdAsync(int id)
        {
            var cycle = await _unitOfWork.Cycle.GetByIdAsync(id);
            return await mapping(cycle);

        }
        public async Task<CycleDetailsForInvestorDTO> GetCycleByIdForInvestorAsync(int Cycleid,int InvestorId)
        {
            var cycleForInvestor = new CycleDetailsForInvestorDTO();
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
            var x = (await _unitOfWork.InvestmentRequest.FindTWithExpression<InvestmentRequest>(ir => ir.InvestorId == InvestorId && ir.CycleId == Cycleid ));
            if (x is not null)
            {
                cycleForInvestor.InvestmentRequestId = x.InvestmentRequestId;
                if (x.RequestStatus == InvestmentStatues.مقبول.ToString())
                {
                    cycleForInvestor.IsInvestorSub = true;
                    var cycleUpdates = await _cycleUpdateService.GetAllUpdatesByCycleIdAsync(Cycleid);
                    cycleForInvestor.cycleUpdates = cycleUpdates.ToList();
                }
            }
            cycleForInvestor.RequestReview = true ? (x is not null && x.RequestStatus == InvestmentStatues.قيد_الانتظار.ToString()) : false;
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
        public async Task<CycleForFarmerDTO> UpdateCycleAsync(UpdateCycleDTO updateCycleDto)
        {
            var existingCycle = await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(updateCycleDto.CycleId, "CycleId",
               c => c.LandParcel,
                c => c.Crop,
                c => c.LandParcel.Farm,
                c => c.LandParcel.Farm.Farmer,
                c => c.InvestmentRequests
                );
            if (existingCycle == null) return null;


            _mapper.Map(updateCycleDto, existingCycle);
            _unitOfWork.Cycle.Update(existingCycle);

            if (updateCycleDto.UpdateOpenInvestmentCycleDTO != null)
            {
                updateCycleDto.UpdateOpenInvestmentCycleDTO.CycleId= existingCycle.CycleId;
                updateCycleDto.UpdateOpenInvestmentCycleDTO.OpenInvestmentCycleId=(await _unitOfWork.OpenInvestmentCycle
                    .FindTWithExpression<OpenInvestmentCycle>(o=>o.CycleId==existingCycle.CycleId)).OpenInvestmentCycleId;
               await _openInvestmentCycleService.UpdateOpenInvestmentCycleAsync(updateCycleDto.UpdateOpenInvestmentCycleDTO);
                existingCycle.OpenInvestmentCycle = _mapper.Map<OpenInvestmentCycle>(updateCycleDto.UpdateOpenInvestmentCycleDTO);

            }
            await _unitOfWork.CompleteAsync();
            return await mapping(existingCycle);
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
        private async Task<CycleForFarmerDTO> mapping(Cycle cycle,bool ForFarmer=true)
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
            if (cycle.InvestmentRequests != null&& ForFarmer == true)
            {
                var CurrentInvestors = new List<CurrentInvestors>();
                var ReqestForInvestment = new List<RequestsForInvestment>();
                
                    cycleDto.NumbersOfRequestsInvestments = 0;
                    var investmentOfCycle =await _investmentRequestService.GetAllRequestsForCycle(cycle.CycleId);
                    foreach (var req in investmentOfCycle)
                    {
                        if (req.RequestStatus == InvestmentStatues.مقبول.ToString())
                        {
                            var invest = await _unitOfWork.Investor.FindTWithIncludes<Investor>(req.InvestorId, "InvestorId",
                                i => i.ApplicationUser);
                            var CurrentInvstment = new CurrentInvestors
                            {
                                FullName = invest.ApplicationUser.UserName,
                                InvestmentAmount = req.RequestedAmount,
                            };
                            CurrentInvestors.Add(CurrentInvstment);
                        }
                        else if (req.RequestStatus == InvestmentStatues.قيد_الانتظار.ToString())
                        {
                            cycleDto.NumbersOfRequestsInvestments += 1;

                            var invest = await _unitOfWork.Investor.FindTWithIncludes<Investor>(req.InvestorId, "InvestorId",
                               i => i.ApplicationUser);
                            var requestForInvestment = new RequestsForInvestment
                            {
                                FullName = invest.ApplicationUser.UserName,
                                InvestmentAmount = req.RequestedAmount,
                                TypeOfProfit = req.RequestedProfitType.ToString(),
                                InvestmentRequestId=req.InvestmentRequestId,
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
        private async Task<CycleForInvestorDTO> mappingForInvestor(Cycle cycle, int investorId)
        {
            var cycleDto = _mapper.Map<CycleForInvestorDTO>(cycle);

            // Check if the cycle is open for investment
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

          
            
                var investmentRequest = await _unitOfWork.InvestmentRequest.FindAllWithIncludes<InvestmentRequest>(
                    ir => ir.InvestorId == investorId && ir.CycleId == cycle.CycleId && ir.RequestStatus == InvestmentStatues.مقبول.ToString());

                if (investmentRequest != null)
                {
                    cycleDto.InvestmentOfInvestor = investmentRequest.Sum(i=>i.RequestedAmount);
                        // Determine the cycle's status (نشطه or منتهيه)
                        var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                        var now = TimeZoneInfo.ConvertTime(DateTime.Now, egyptTimeZone);

                        if (cycle.EndDate <= now)
                        {
                            cycleDto.Statue = "منتهيه"; // Cycle has ended
                        }
                        else if (cycle.StartDate <= now)
                        {
                            cycleDto.Statue = "نشطه"; // Cycle is active
                        }
                        else
                        {
                            cycleDto.Statue = "قيد الانتظار"; // Cycle has not started yet
                        }
                 }
               

            // Calculate the remaining time message
            cycleDto.TimeToStart = GetRemainingTimeMessage(cycleDto.StartDate, cycleDto.EndDate);

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
