using CityRoots.Core.DTOs.PayPal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly PayPalService _payPalService;

        public PayPalController(PayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PayPalPaymentRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var paymentUrl = await _payPalService.CreatePaymentLink(request.Amount, request.SellerEmail,userId, request.CycleId);
                if (paymentUrl == null)
                {
                    return BadRequest(new { message = "Failed to generate payment link" });
                }
                return Ok(new { url = paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("create-paymentforMerchant")]
        public async Task<IActionResult> CreatePaymentForMerchant([FromBody] PayPalRequestforMerchant request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var paymentUrl = await _payPalService.CreatePaymentLink(request.Amount, request.SellerEmail,userId,0,request.HarvestId);
                if (paymentUrl == null)
                {
                    return BadRequest(new { message = "Failed to generate payment link" });
                }
                return Ok(new { url = paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string token)
        {
            await _payPalService.updateTransaction(token, "مقبول");
            return Redirect("https://graduation-project-y3x4.vercel.app/payment-success");
        }

        [HttpGet("cancel")]
        public async Task<IActionResult> PaymentCancelled([FromQuery] string token)
        {
            await _payPalService.updateTransaction(token, "مرفوض");
            return Redirect("https://graduation-project-y3x4.vercel.app/payment-cancel");
        }

    }

}


