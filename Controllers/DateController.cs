using Microsoft.AspNetCore.Mvc;
using OSOS_Task_ASP.Dtos;
using OSOS_Task_ASP.Interfaces;

namespace OSOS_Task_ASP.Controllers
{
    [Route("api/date")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private readonly IDateService _dateService;

        public DateController(IDateService dateService)
        {
            _dateService = dateService;
        }

        [HttpPost]
        public async Task<IActionResult> GetEndDate([FromBody] DateRequestDto request)
        {

            if (request == null )
                return BadRequest("Invalid inputs");
            
            if (string.IsNullOrWhiteSpace(request.StartDate.ToString()))
                return BadRequest("Invalid input for start date");

            if (request.WorkingDays <= 0)
                return BadRequest("Invalid input for working days");
            var response = await _dateService.CalculateEndDate(request.StartDate, request.WorkingDays);

            if(response == null)
                return StatusCode(500, new {error = "An unexpected error occured"});

            return Ok(response);
        }
    }
}