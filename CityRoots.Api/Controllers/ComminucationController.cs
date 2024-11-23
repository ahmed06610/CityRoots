using CityRoots.Core.DTOs.FeedBack;
using CityRoots.Core.Helpers;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComminucationController : ControllerBase
    {
        private readonly ICommunicationService communicationService;
        public ComminucationController(ICommunicationService communicationService)
        {
            this.communicationService = communicationService;

        }
        [HttpGet]
        public async Task<IActionResult> GetFeedBacks()
        {

            try
            {
                var feedbacks = await communicationService.GetAll();
                return Ok(feedbacks);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetbyId([FromRoute]int Id)
        {
            try
            {
                var feedback = await communicationService.GetById(Id);
                return Ok(feedback);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] FeedBackRequest feedback)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var UpdatedfeedBack = await communicationService.Update(Id, feedback);
                return Ok(UpdatedfeedBack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FeedBackRequest feedback)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var addedFeedBack = await communicationService.Add(feedback);
                return Ok(addedFeedBack);
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
                await communicationService.Delete(Id);
                return Ok("Deleted");




            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("SendSupprt")]
        public async Task<IActionResult> SendSupport(Support support)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try { 
           await communicationService.SendSupportAsync(support);
            return Ok("Support Sent Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}