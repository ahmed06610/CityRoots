using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Purchaserequest;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequestController : ControllerBase
    {
        private readonly IPurchaseRequestService _purchaseRequestService;
        public PurchaseRequestController(IPurchaseRequestService purchaseRequestService)
        {
            _purchaseRequestService = purchaseRequestService;
        }
        [HttpGet("GetAllRequestsForHarvest/{harvestId}")]
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
        public async Task<IActionResult> GetAllForInvestor(int MerchantId)
        {
            try
            {
                return Ok(await _purchaseRequestService.GetAllRequestsForMerchant(MerchantId));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }
        [HttpGet("GetPurchaseRequest/{id}")]
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
        public async Task<IActionResult> CreatePurchaseRrquest(CreatePurchaseRrquest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                return Ok(await _purchaseRequestService.CreatePurchaseRequest(request));

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
                await _purchaseRequestService.Delete(Id);
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
                await _purchaseRequestService.UpdateRequest(Id, PurchaseRequestStatus.مقبول.ToString());

                return Ok("Approved");
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
                await _purchaseRequestService.UpdateRequest(Id, PurchaseRequestStatus.مرفوض.ToString());
                return Ok("Declined");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

