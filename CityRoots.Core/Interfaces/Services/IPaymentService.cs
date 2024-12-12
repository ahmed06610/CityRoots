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
        Task<List<PaymentDTO>> GetPaymentsAsync(PaymentFilterDTO filter);
        Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId);
    }
}
