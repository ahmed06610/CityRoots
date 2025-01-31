using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Payment;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PaymentDetailsDTO>> GetPaymentsAsync(PaymentFilterDTO filter)
        {
            var query =await _unitOfWork.Payment.FindAllWithIncludes<Payment>(p=>(p.PayerId==filter.Id||p.PayeeId==filter.Id),
                p => p.Payer,
                p => p.Payee,
                 p => p.Cycle,
                p => p.Harvest,
                p => p.Harvest.Crop
                );

            // Apply filters
            if (!string.IsNullOrEmpty(filter.Type))
                query = query.Where(p => p.Type == filter.Type);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(p => p.Statue == filter.Status);

            if (filter.StartDate.HasValue)
                query = query.Where(p => p.PaymentDate >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(p => p.PaymentDate <= filter.EndDate.Value);

            /* var payments =  query.Select(p => new PaymentDTO
                                        {
                                            PaymentId = p.PaymentId,
                                            PaymentDate = p.PaymentDate,
                                            Amount = p.Amount,
                                            Type = p.Type,
                                            Payer = p.Payer.Name,
                                            Payee = p.Payee.Name,
                                            PaymentMethod = p.PaymentMethod,
                                            Status = p.Statue
                                        }).ToList();*/
            var payments = new List<PaymentDetailsDTO>();

            foreach (var payment in query)
            {
                if (payment == null) return null;

                AssociatedCycleDTO assoc = null;
                AssociatedHarvestDTO assoh = null;

                if (payment.Type == PaymentType.Investment.ToString())
                {
                    assoc = new AssociatedCycleDTO
                    {
                        CycleId = payment.Cycle.CycleId,
                        CycleName = payment.Cycle.CycleName,
                    };
                }
                else
                {
                    assoh = new AssociatedHarvestDTO
                    {
                        HarvestId = payment.Harvest.HarvestId,
                        HarvestName = payment.Harvest.Crop.Name,
                    };
                }
                var pay= new PaymentDetailsDTO
                {
                    PaymentId = payment.PaymentId,
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.Amount,
                    Type = payment.Type,
                    PayerName = payment.Payer.Name,
                    PayerEmail = payment.Payer.Email,
                    PayeeName = payment.Payee.Name,
                    PayeeEmail = payment.Payee.Email,
                    PaymentMethod = payment.PaymentMethod,
                    Status = payment.Statue,
                    AssociatedCycle = assoc,
                    AssociatedHarvest = assoh
                };
                payments.Add(pay);
            }

            return payments;
        }

        public async Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId)
        {
            var payment = await _unitOfWork.Payment.FindTWithIncludes<Payment>(paymentId, "PaymentId",
                p => p.Payer,
                p => p.Payee,
                p => p.Cycle,
                p => p.Harvest,
                p => p.Harvest.Crop
                        );
            if (payment == null) return null;

            AssociatedCycleDTO assoc = null;
            AssociatedHarvestDTO assoh = null;

            if (payment.Type == PaymentType.Investment.ToString())
            {
                assoc = new AssociatedCycleDTO
                {
                    CycleId = payment.Cycle.CycleId,
                    CycleName = payment.Cycle.CycleName,
                };
            }
            else
            {
                assoh = new AssociatedHarvestDTO
                {
                    HarvestId = payment.Harvest.HarvestId,
                    HarvestName = payment.Harvest.Crop.Name,
                };
            }
            return new PaymentDetailsDTO
            {
                PaymentId = payment.PaymentId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                Type = payment.Type,
                PayerName = payment.Payer.Name,
                PayerEmail = payment.Payer.Email,
                PayeeName = payment.Payee.Name,
                PayeeEmail = payment.Payee.Email,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Statue,
                AssociatedCycle = assoc,
                AssociatedHarvest = assoh
            };
        }


        public async Task DeletePaymentsByCycleIdAsync(int cycleId)
        {
            // Find all payments related to the cycleId
            var payments = await _unitOfWork.Payment.FindAllAsync(p => p.CycleId == cycleId);

            if (payments.Any())
            {
                // Remove related payments
                foreach (var payment in payments)
                {
                   await _unitOfWork.Payment.DeleteAsync(payment);

                }
                await _unitOfWork.CommitAsync();  // Save changes to the database
            }
        }
    }
}
