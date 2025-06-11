using CityRoots.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CityRoots.Core.Services
{
    public class PaymentBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentBackgroundService(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public async Task ProcessPaymentsAsync()
        {

            var pendingPayments = await _unitOfWork.Payment.FindAllAsync(
       p => p.Statue == "قيد الانتظار" &&
Microsoft.EntityFrameworkCore.EF.Functions.DateDiffDay(p.PaymentDate, DateTime.UtcNow) > 3
   );

            foreach (var payment in pendingPayments)
            {
                
                payment.Statue = "مرفوض"; 
                _unitOfWork.Payment.Update(payment);
            }

            if (pendingPayments.Any())
            {
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
