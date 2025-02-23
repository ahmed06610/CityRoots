using CityRoots.Core.Const;
using CityRoots.Core.DTOs.InvestmentRequests;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentRequestController : ControllerBase
    {
        private readonly IInvestmentRequestService _investmentRequestService;
        public InvestmentRequestController(IInvestmentRequestService investmentRequestService)
        {
            _investmentRequestService = investmentRequestService;
        }
        [HttpGet("GetAllRequestForCycle/{cycleId}")]
        public async Task<IActionResult> GetAllRequestForCycle(int cycleId)
        {

            try
            {
                return Ok(await _investmentRequestService.GetAllRequestsForCycle(cycleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllForInvestor/{InvestorId}")]
        public async Task<IActionResult> GetAllForInvestor(int InvestorId)
        {
            try
            {
                return Ok(await _investmentRequestService.GetAllRequestsForInvestor(InvestorId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }
        [HttpGet("GetInvestmentRequest/{id}")]
        public async Task<IActionResult> GetInvestmentRequest(int id)
        {
            try
            {
                return Ok(await _investmentRequestService.GetSpeceficInvestmentRequest(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateInvestmentRequest(CreateInvestmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _investmentRequestService.CreateInvestmentRequest(request);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _investmentRequestService.DeleteInvestmentRequest(Id);
                return Ok("Deleted");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Approved/{Id}")]
        public async Task<IActionResult> ApproveTheRequest(int Id)
        {
            try
            {
               
                return Ok(await _investmentRequestService.UpdateInvestmentRequest(Id, InvestmentStatues.مقبول.ToString()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Declined/{Id}")]
        public async Task<IActionResult> DeclineTheRequest(int Id)
        {
            try
            {
                await _investmentRequestService.UpdateInvestmentRequest(Id, InvestmentStatues.مرفوض.ToString());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
 }