using CityRoots.Core.DTOs.Payment;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
       // private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
         //   _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> GetPaymentsForFarmer([FromQuery] PaymentFilterDTO filter)
        {
            var payments = await _paymentService.GetPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentDetails(int id)
        {
            var payment = await _paymentService.GetPaymentDetailsAsync(id);
            if (payment == null) return NotFound("Payment not found.");

            return Ok(payment);
        }
        [HttpGet("GetInvestorPayments")]
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> GetPaymentsForInvestor()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Ok(await _paymentService.GetInvestorPaymentReportsAsync(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetInvesstorPaymentDetails/{PaymentId}")]
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> GetInvesstorPaymentDetails(int PaymentId)
        {
            try
            {
                return Ok(await _paymentService.GetInvestorPaymentReportDetails(PaymentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetMerchantPayments")]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> GetPaymentsForMerchant()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                

                return Ok(await _paymentService.GetMerchantPaymentReports(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetMerchantPaymentDetails/{PaymentId}")]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> GetMerchantPaymentDetails(int PaymentId)
        {
            try
            {
                return Ok(await _paymentService.GetMerchantPaymentReportDetails(PaymentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}