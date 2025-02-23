using CityRoots.Core.Interfaces;
using CityRoots.Core.Models.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class InteractionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InteractionsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InteractionOfInvestor> VisitCycle(int InvestorId,int CycleId)
        {
            var visC = new InteractionOfInvestor
            {
                InvestorId = InvestorId,
                CycleId = CycleId,
            };
           await _unitOfWork.InteractionOfInvestor.AddAsync(visC);
           await _unitOfWork.CompleteAsync();
            return visC;
        }
        public async Task<InteractionOfMerchant> VisitHarvest(int MerchantId, int HarvestId)
        {
            var visH = new InteractionOfMerchant
            {
                MerchantId = MerchantId,
                HarvestId = HarvestId,
            };
            await _unitOfWork.InteractionOfMerchant.AddAsync(visH);
            await _unitOfWork.CompleteAsync();
            return visH;
        }
    }
}
