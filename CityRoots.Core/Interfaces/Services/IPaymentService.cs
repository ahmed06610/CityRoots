using CityRoots.Core.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentResultsDTO> GetPaymentsAsync(PaymentFilterDTO filter);
        Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId);
        Task DeletePaymentsByCycleIdAsync(int cycleId);
        Task<InvestorPaymentReportsResponseDto> GetInvestorPaymentReportsAsync(string userId);
        Task<MerchantPaymentReportsDto> GetMerchantPaymentReports(string userId);
    }
}
