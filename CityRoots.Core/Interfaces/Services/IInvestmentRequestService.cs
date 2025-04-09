using CityRoots.Core.DTOs.InvestmentRequests;
using CityRoots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IInvestmentRequestService
    {
        Task<List<InvestmentrequestDisplay>>GetAllRequestsForInvestor(int InvestorId);
        Task<List<InvestmentrequestDisplay>> GetAllRequestsForCycle(int cycleId);
        Task<InvestmentrequestDisplay> GetSpeceficInvestmentRequest(int Id);


        Task<InvestmentRequest> CreateInvestmentRequest(CreateInvestmentRequest request,int investorId);
        Task<InvestmentRequest> UpdateInvestmentRequest(int Id,string status);
        Task DeleteInvestmentRequest(int id);
    }
}
