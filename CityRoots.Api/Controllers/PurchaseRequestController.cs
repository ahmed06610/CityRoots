using CityRoots.Api.Helpers;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Purchaserequest;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequestController : ControllerBase
    {
        private readonly IPurchaseRequestService _purchaseRequestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IHarvestNotificationService _harvestNotificationService;
        public PurchaseRequestController(IPurchaseRequestService purchaseRequestService,IBackgroundJobClient backgroundJobClient, IHarvestNotificationService harvestNotificationService)
        {
            _purchaseRequestService = purchaseRequestService;
            _backgroundJobClient = backgroundJobClient;
            _harvestNotificationService = harvestNotificationService;
        }
        [HttpGet("GetAllRequestsForHarvest/{harvestId}")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> GetAllRequestForCycle(int harvestId)
        {

            try
            {
                return Ok(await _purchaseRequestService.GetAllRequestsForHarvest(harvestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllRequestsForMerchant/{MerchantId}")]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> GetAllForInvestor()
        {
            var merchantId=User.GetLoggedInId();
            if(merchantId is null) return Unauthorized();
            try
            {
                return Ok(await _purchaseRequestService.GetAllRequestsForMerchant(merchantId.Value));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }
        [HttpGet("GetPurchaseRequest/{id}")]
        [Authorize]

        public async Task<IActionResult> GetPurchaseRequest(int id)
        {
            try
            {
                return Ok(await _purchaseRequestService.GetSpecificRequest(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> CreatePurchaseRequest(CreatePurchaseRrquest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var merchantId = User.GetLoggedInId();
            if (merchantId is null) return Unauthorized();
            try
            {
                var _request = await _purchaseRequestService.CreatePurchaseRequest(request, merchantId.Value);
                _backgroundJobClient.Enqueue(()=>
                _harvestNotificationService.notifyOnPurchaseRequest(_request.HarvestId,merchantId.Value,_request));
                return Ok(_request);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Merchant")]

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _purchaseRequestService.Delete(Id);
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
            var userName = User?.FindFirst("NameOfuser")?.Value;
            if (userName is null) return Unauthorized();

            try
            {
               var harvest= await _purchaseRequestService.UpdateRequest(Id, PurchaseRequestStatus.مقبول.ToString());


                _backgroundJobClient.Enqueue(() =>
                _harvestNotificationService.NotifyMerchantOfpurchaseResponseAsync(userName, harvest, PurchaseRequestStatus.مقبول.ToString()));
                if(harvest.status==HarvestStatus.منتهي.ToString())
                    _backgroundJobClient.Enqueue(() =>
                _harvestNotificationService.NotifyFinishedYield(harvest));

                return Ok("Approved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Declined/{Id}")]
        [Authorize(Roles = "Farmer")]

        public async Task<IActionResult> DeclineTheRequest(int Id)
        {
            var userName = User?.FindFirst("NameOfuser")?.Value;
            if (userName is null) return Unauthorized();
            try
            {
               var harvest= await _purchaseRequestService.UpdateRequest(Id, PurchaseRequestStatus.مرفوض.ToString());
                _backgroundJobClient.Enqueue(() =>
               _harvestNotificationService.NotifyMerchantOfpurchaseResponseAsync(userName, harvest, PurchaseRequestStatus.مرفوض.ToString()));
                return Ok("Declined");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

