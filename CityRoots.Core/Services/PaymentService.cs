using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Payment;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<PaymentResultsDTO> GetPaymentsAsync(PaymentFilterDTO filter)
        {
            var query = await _unitOfWork.Payment.FindAllWithIncludes<Payment>(
                p => p.PayerId == filter.Id || p.PayeeId == filter.Id,
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

            var payments = new List<PaymentDetailsDTO>();
            var summaryDict = new Dictionary<int, (List<decimal> Investments, List<decimal> Purchases)>();

            foreach (var payment in query)
            {
                if (payment == null ) continue;

                AssociatedCycleDTO assoc = null;
                AssociatedHarvestDTO assoh = null;

                var year = payment.PaymentDate.Year;
                var month = payment.PaymentDate.Month - 1; // 0-based index

                // Initialize summary data for the year if not exists
                if (!summaryDict.ContainsKey(year))
                {
                    summaryDict[year] = (
                        Investments: new List<decimal>(new decimal[12]),
                        Purchases: new List<decimal>(new decimal[12])
                    );
                }

                if (payment.Type == PaymentType.Investment.ToString()|| payment.Type == "استثمار")
                {
                    assoc = new AssociatedCycleDTO
                    {
                        CycleId = payment.Cycle.CycleId,
                        CycleName = payment.Cycle.CycleName,
                    };
                    if (payment.Statue == "مقبول")
                    {
                        // Add to investments
                        summaryDict[year].Investments[month] += payment.Amount;
                    }
                }
                else
                {
                    assoh = new AssociatedHarvestDTO
                    {
                        HarvestId = payment.Harvest.HarvestId,
                        HarvestName = payment.Harvest.Crop.Name,
                    };
                    if (payment.Statue == "مقبول")
                    {
                        // Add to purchases
                        summaryDict[year].Purchases[month] += payment.Amount;
                    }
                }

                payments.Add(new PaymentDetailsDTO
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
                });
            }

            // Convert the summary dictionary to PaymentSummaryDTO list
            var paymentsSummary = summaryDict
                .OrderBy(x => x.Key)
                .Select(x => new PaymentSummaryDTO
                {
                    Year = x.Key,
                    InvestmentsPerMonth = x.Value.Investments,
                    PurchasesPerMonth = x.Value.Purchases
                })
                .ToList();

            return new PaymentResultsDTO
            {
                Payments = payments,
                PaymentsSummary = paymentsSummary
            };
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
        //For Investors
        public async Task<InvestorPaymentReportsResponseDto> GetInvestorPaymentReportsAsync(string userId)
        {
            // Get all investment payments for the investor
            var payments = await _unitOfWork.Payment.FindAllWithIncludes<Payment>(
                x => x.PayerId == userId||x.PayeeId==userId && x.Type == PaymentType.Investment.ToString(),
                x => x.Payee,
                x => x.Cycle,
                x => x.Payer
            );

            if (!payments.Any())
            {
                return new InvestorPaymentReportsResponseDto
                {
                    Payments = new List<InvestorPaymentDetailDto>(),
                    PaymentsSummary = new List<PaymentInvestorSummaryDto>()
                };
            }

            // Group payments by year and month to calculate InvestmentsPerMonth
            var paymentsByYear = payments
                .GroupBy(p => p.PaymentDate.Year)
                .OrderBy(g => g.Key);

            var paymentSummary = new List<PaymentInvestorSummaryDto>();

            foreach (var yearGroup in paymentsByYear)
            {
                var year = yearGroup.Key;
                var investmentsPerMonth = new decimal[12];

                // Fill monthly data
                foreach (var payment in yearGroup)
                {
                    if (payment == null || payment.Statue != "مقبول") continue;

                    var month = payment.PaymentDate.Month - 1; // 0-based index
                    investmentsPerMonth[month] += payment.Amount;
                }

                paymentSummary.Add(new PaymentInvestorSummaryDto
                {
                    Year = year,
                    InvestmentsPerMonth = investmentsPerMonth.ToList(),
                });
            }

            // Map the payment details
            var paymentDetails = _mapper.Map<List<InvestorPaymentDetailDto>>(payments);

            return new InvestorPaymentReportsResponseDto
            {
                Payments = paymentDetails,
                PaymentsSummary = paymentSummary
            };
        }


        public async Task<MerchantPaymentReportsDto> GetMerchantPaymentReports(string userId)
        {
            // Get all merchant payments (purchases)
            var payments = await _unitOfWork.Payment.FindAllWithIncludes<Payment>(
                x => x.PayerId == userId || x.PayeeId == userId && x.Type == PaymentType.Purchase.ToString(),
                x => x.Payer,  // The buyer in this case
                x => x.Harvest,
                x => x.Harvest.Crop,
                x => x.Payee
            );

            if (!payments.Any())
            {
                return new MerchantPaymentReportsDto
                {
                    Payments = new List<MerchantPaymentDetailDto>(),
                    PaymentsSummary = new List<PaymentMerchantSummaryDto>()
                };
            }

            // Group payments by year and month to calculate PurchasesPerMonth
            var paymentsByYear = payments
                .GroupBy(p => p.PaymentDate.Year)
                .OrderBy(g => g.Key);

            var paymentSummary = new List<PaymentMerchantSummaryDto>();

            foreach (var yearGroup in paymentsByYear)
            {
                var year = yearGroup.Key;
                var purchasesPerMonth = new decimal[12];

                // Fill monthly data
                foreach (var payment in yearGroup)
                {
                    if (payment == null || payment.Statue != "مقبول") continue;

                    var month = payment.PaymentDate.Month - 1; // 0-based index
                    purchasesPerMonth[month] += payment.Amount;
                }

                paymentSummary.Add(new PaymentMerchantSummaryDto
                {
                    Year = year,
                    PurchasesPerMonth = purchasesPerMonth.ToList()
                });
            }

            // Map the payment details
            var paymentDetails = _mapper.Map<List<MerchantPaymentDetailDto>>(payments);

            return new MerchantPaymentReportsDto
            {
                Payments = paymentDetails,
                PaymentsSummary = paymentSummary
            };
        }
    }
}
