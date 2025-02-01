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
                var paymentUrl = await _payPalService.CreatePaymentLink(request.Amount, request.SellerEmail);
            if (paymentUrl == null)
            {
                return BadRequest(new { message = "Failed to generate payment link" });
            }
            return Ok(new { url = paymentUrl });
        }
    }
}

