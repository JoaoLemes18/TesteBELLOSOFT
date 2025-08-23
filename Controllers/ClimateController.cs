using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClimateController : ControllerBase
    {
        private readonly ClimateService _service;

        public ClimateController(ClimateService service) => _service = service;

        [HttpPost("sync")]
        public async Task<IActionResult> Sync([FromBody] ClimateSyncRequest request, CancellationToken ct)
        {
            var rec = await _service.FetchAndSaveAsync(request, ct);
            return Ok(new { message = "Sincronizado com sucesso.", record = rec });
        }

        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            var list = await _service.GetHistoryAsync();
            return Ok(list);
        }
    }
}
