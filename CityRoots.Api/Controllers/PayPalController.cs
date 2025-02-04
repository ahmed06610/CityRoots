using CityRoots.Core.DTOs.PayPal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                var paymentUrl = await _payPalService.CreatePaymentLink(request.Amount, request.SellerEmail,request.CycleId,request.userId);
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
          await _payPalService.updateTransaction(token,"مقبول");
               return Redirect("https://localhost:7109");  // Redirect to your frontend
          }

        [HttpGet("cancel")]
        public async Task <IActionResult> PaymentCancelled([FromQuery] string token)
        {
            await _payPalService.updateTransaction(token, "مرفوض");

            return Redirect("https://localhost:7109");  // Redirect back to your site
        }
    }

}


