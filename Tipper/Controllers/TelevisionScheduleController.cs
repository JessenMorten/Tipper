using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelevisionScheduleService;

namespace Tipper.Controllers
{
    [ApiController]
    [Route("televisionschedule")]
    public class TelevisionScheduleController : ControllerBase
    {
        private readonly ILogger<TelevisionScheduleController> _logger;

        private readonly ITelevisionScheduleService _televisionScheduleService;

        public TelevisionScheduleController(ILogger<TelevisionScheduleController> logger, ITelevisionScheduleService televisionScheduleService)
        {
            _logger = logger;
            _televisionScheduleService = televisionScheduleService;
        }

        [HttpGet("channels")]
        public async Task<IActionResult> FetchAllChannels()
        {
            try
            {
                var channels = await _televisionScheduleService.FetchAllChannels(HttpContext.RequestAborted);
                return Ok(channels);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch channels");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("program/{channelId}/{date}")]
        public async Task<IActionResult> FetchProgram(string channelId, DateTime date)
        {
            try
            {
                var program = await _televisionScheduleService
                    .FetchProgramDescriptions(channelId, date, HttpContext.RequestAborted);

                if (!program.Any())
                {
                    return NotFound();
                }

                return Ok(program);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch program");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
