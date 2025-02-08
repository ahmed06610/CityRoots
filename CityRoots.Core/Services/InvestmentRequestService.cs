using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.InvestmentRequests;
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
    public class InvestmentRequestService : IInvestmentRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOpenInvestmentCycleService _openInvestmentCycle;
        public InvestmentRequestService(IUnitOfWork unitOfWork, IMapper mapper, IOpenInvestmentCycleService openInvestmentCycle)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _openInvestmentCycle = openInvestmentCycle;
        }
        public async Task<InvestmentRequest> CreateInvestmentRequest(CreateInvestmentRequest request)
        {
            var investmentRequest=_mapper.Map<InvestmentRequest>(request);
            investmentRequest.RequestDate = DateTime.Now;
            investmentRequest.RequestStatus = "قيد_الانتظار";
            await _unitOfWork.InvestmentRequest.AddAsync(investmentRequest);
            await _unitOfWork.CompleteAsync();
            return investmentRequest;
        }

        public async Task DeleteInvestmentRequest(int id)
        {
            var request=await _unitOfWork.InvestmentRequest.GetByIdAsync(id);
            if (request is null)
                throw new Exception($"No requests with this Id {id}");
            await _unitOfWork.InvestmentRequest.DeleteAsync(request);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<InvestmentrequestDisplay>> GetAllRequestsForCycle(int cycleId)
        {
            var requests = (await _unitOfWork.InvestmentRequest.FindAllWithIncludes<InvestmentRequest>(x => x.CycleId == cycleId,
               x => x.Cycle,
               x => x.Cycle.LandParcel,
               x => x.Cycle.LandParcel.Farm,
               x => x.Cycle.LandParcel.Farm.Farmer,
                x => x.Cycle.LandParcel.Farm.Farmer.ApplicationUser)).ToList();
            return _mapper.Map<List<InvestmentrequestDisplay>>(requests);
        }

        public  async Task<List<InvestmentrequestDisplay>> GetAllRequestsForInvestor(int InvestorId)
        {
            var requests = (await _unitOfWork.InvestmentRequest.FindAllWithIncludes<InvestmentRequest>(x => x.InvestorId == InvestorId,
               x => x.Cycle,
               x => x.Cycle.LandParcel,
               x => x.Cycle.LandParcel.Farm,
               x => x.Cycle.LandParcel.Farm.Farmer,
               x => x.Cycle.LandParcel.Farm.Farmer.ApplicationUser)).ToList();
            return _mapper.Map<List<InvestmentrequestDisplay>>(requests);

        }

        public async Task<InvestmentrequestDisplay> GetSpeceficInvestmentRequest(int Id)
        {
            var request = await _unitOfWork.InvestmentRequest.FindTWithIncludes<InvestmentRequest>(Id, "InvestmentRequestId",
                x => x.Cycle,
                x => x.Cycle.LandParcel,
                x => x.Cycle.LandParcel.Farm,
                x => x.Cycle.LandParcel.Farm.Farmer,
                x => x.Cycle.LandParcel.Farm.Farmer.ApplicationUser);
            return _mapper.Map<InvestmentrequestDisplay>(request);
        }

        public async Task<InvestmentRequest> UpdateInvestmentRequest(int id,string status)
        {
            var request = await _unitOfWork.InvestmentRequest.GetByIdAsync(id);

            if (request is null)
                throw new Exception($"No requests with this Id {id}");
            if(status== InvestmentStatues.مقبول.ToString())
            {
                var op=(await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(request.CycleId,"CycleId",c=>c.OpenInvestmentCycle)).OpenInvestmentCycle;
                op.CurrentTotalInvestment += request.RequestedAmount;
                op.CurrentInvestorCount++;
                _unitOfWork.OpenInvestmentCycle.Update(op);
            }
            request.RequestStatus = status;
            _unitOfWork.InvestmentRequest.Update(request);
            await _unitOfWork.CompleteAsync();
            request.Cycle = null;
            return request;
        }
    }
}
