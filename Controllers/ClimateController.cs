using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClimateController : ControllerBase
    {
        private readonly IClimateService _service;

        public ClimateController(IClimateService service)
        {
            _service = service;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync([FromBody] ClimateSyncRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDtos<string>.Fail("Dados inválidos."));

            var rec = await _service.FetchAndSaveAsync(request, ct);
            return Ok(ApiResponseDtos<object>.Ok(rec, "Sincronizado com sucesso."));
        }

        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            var list = await _service.GetHistoryAsync();
            return Ok(ApiResponseDtos<object>.Ok(list, "Histórico retornado com sucesso."));
        }
    }
}
