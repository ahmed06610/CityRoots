using CityRoots.Api.Helpers;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.InvestmentRequests;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentRequestController : ControllerBase
    {
        private readonly IInvestmentRequestService _investmentRequestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ICycleNotificationService _cycleNotificationService;

        public InvestmentRequestController(IInvestmentRequestService investmentRequestService,IBackgroundJobClient backgroundJobClient, ICycleNotificationService cycleNotificationService)
        {
            _investmentRequestService = investmentRequestService;
            _backgroundJobClient = backgroundJobClient;
            _cycleNotificationService = cycleNotificationService;
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
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> GetAllForInvestor()
        {
            var investorId=User.GetLoggedInId();
            
            if(investorId is null) return Unauthorized();
            try
            {

                return Ok(await _investmentRequestService.GetAllRequestsForInvestor(investorId.Value));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }
        [HttpGet("GetInvestmentRequest/{id}")]
        [Authorize(Roles = "Investor,Farmer")]

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
        [Authorize(Roles = "Investor")]

        public async Task<IActionResult> CreateInvestmentRequest(CreateInvestmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var investorId = User.GetLoggedInId();

            if (investorId is null) return Unauthorized();

            try
            {
               var _request= await _investmentRequestService.CreateInvestmentRequest(request,investorId.Value);
                var farmerId = _request.Cycle.LandParcel.Farm.Farmer.ApplicationUserId;
                _backgroundJobClient.Enqueue( ()=>
      _cycleNotificationService.NotifyInvestmentRequestAsync(request.CycleId,farmerId,investorId.Value, request.RequestedAmount));
                return Ok(request);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Investor")]

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
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> ApproveTheRequest(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var userName = User?.FindFirst("NameOfuser")?.Value;
            try
            {
                var request = await _investmentRequestService.UpdateInvestmentRequest(Id, InvestmentStatues.مقبول.ToString());
                _backgroundJobClient.Enqueue(() =>
                _cycleNotificationService.NotifyInvestorOfInvestmentResponseAsync(request.CycleId, userName, request.InvestorId, InvestmentStatues.مقبول.ToString()));
                
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Declined/{Id}")]
        [Authorize(Roles="Farmer")]
        public async Task<IActionResult> DeclineTheRequest(int Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var userName = User?.FindFirst("NameOfuser")?.Value;
            try
            {
               var request= await _investmentRequestService.UpdateInvestmentRequest(Id, InvestmentStatues.مرفوض.ToString());
                _backgroundJobClient.Enqueue(() =>
               _cycleNotificationService.NotifyInvestorOfInvestmentResponseAsync(request.CycleId, userName, request.InvestorId, InvestmentStatues.مرفوض.ToString()));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
 }