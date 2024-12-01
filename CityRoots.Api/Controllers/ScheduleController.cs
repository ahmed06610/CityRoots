using CityRoots.Core.DTOs.Schedule;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [HttpGet("GetAllTasks/{CycleId}")]
        public async Task<IActionResult> GetAll(int CycleId)
        {
            try
            {

                return Ok(await _scheduleService.GetAll(CycleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetTask/{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                return Ok(await _scheduleService.Get(Id));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddTask")]
        public async Task<IActionResult> Add(AddScheduleDto addScheduleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                return Ok(await _scheduleService.Add(addScheduleDto));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }
        [HttpPut("UpdateTask")]
        public async Task<IActionResult> Update(UpdateScheduleDTO updateScheduleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try {

                return Ok(await _scheduleService.update(updateScheduleDTO));
            }
            catch (Exception ex) {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try {
                await _scheduleService.Delete(Id);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("MakeTaskCompeleted/{Id}")]
        public async Task<IActionResult> CompeleteTask(int Id)

        {
            try { 
            await _scheduleService.CompelteTask(Id);
                return Ok("Compeleted");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }
    }
}
