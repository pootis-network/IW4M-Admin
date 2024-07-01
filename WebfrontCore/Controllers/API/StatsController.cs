using System;
using System.Threading.Tasks;
using IW4MAdmin.Plugins.Stats.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibraryCore.Dtos;
using SharedLibraryCore.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace IW4MAdmin.WebfrontCore.Controllers.API
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IResourceQueryHelper<StatsInfoRequest, StatsInfoResult> _statsQueryHelper;

        public StatsController(ILogger<StatsController> logger, IResourceQueryHelper<StatsInfoRequest, StatsInfoResult> statsQueryHelper)
        {
            _statsQueryHelper = statsQueryHelper;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{clientId}")]
        public async Task<IActionResult> ClientStats(int clientId)
        {
            if (clientId < 1 || !ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Messages = new[] { $"Client Id must be between 1 and {int.MaxValue}" }
                });

            }

            var request = new StatsInfoRequest()
            {
                ClientId = clientId
            };

            try
            {
                var result = await _statsQueryHelper.QueryResource(request);

                if (result.RetrievedResultCount == 0)
                {
                    return NotFound();
                }

                return Ok(result.Results);
            }

            catch (Exception e)
            {
                _logger.LogWarning(e, "Could not get client stats for client id {clientId}", clientId);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Messages = new[] { e.Message }
                });
            }
        }
    }
}
