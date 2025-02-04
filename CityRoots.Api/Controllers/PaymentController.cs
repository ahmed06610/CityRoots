using CityRoots.Core.DTOs.Payment;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsForFarmer([FromQuery] PaymentFilterDTO filter)
        {
            var payments = await _paymentService.GetPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentDetails(int id)
        {
            var payment = await _paymentService.GetPaymentDetailsAsync(id);
            if (payment == null) return NotFound("Payment not found.");

            return Ok(payment);
        }
        [HttpGet("GetInvestorPayments/{userId}")]
        public async Task<IActionResult> GetPaymentsForInvestor(string userId)
        {
            try
            {
                return Ok(await _paymentService.GetInvestorPaymentReportsAsync(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetInvesstorPaymentDetails/{PaymentId}")]
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
    }
}