using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace API.Controllers
{
    /// <summary>
    /// Endpoints para sincronização e consulta de registros climáticos.
    /// Requer autenticação via JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClimateController : ControllerBase
    {
        private readonly IClimateService _service;

        public ClimateController(IClimateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Consulta a Open-Meteo e salva o clima atual no banco.
        /// </summary>
        [HttpPost("sync")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClimateSyncOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ClimateSyncBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ClimateSyncUnauthorizedExample))]
        public async Task<IActionResult> Sync([FromBody] ClimateSyncRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDtos<string>.Fail("Dados inválidos."));

            var rec = await _service.FetchAndSaveAsync(request, ct);
            return Ok(ApiResponseDtos<ClimateRecord>.Ok(rec, "Clima sincronizado com sucesso."));
        }

        /// <summary>
        /// Retorna o histórico mais recente de registros climáticos.
        /// </summary>
        [HttpGet("history")]
        [ProducesResponseType(typeof(ApiResponseDtos<IEnumerable<ClimateRecord>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> History()
        {
            var list = await _service.GetHistoryAsync();
            return Ok(ApiResponseDtos<IEnumerable<ClimateRecord>>.Ok(list, "Histórico retornado com sucesso."));
        }
    }
}
