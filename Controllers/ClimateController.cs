using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Endpoints para sincronização e consulta de registros climáticos.
    /// Requer autenticação via JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClimateController : ControllerBase
    {
        private readonly IClimateService _service;

        /// <summary>
        /// Construtor que injeta o serviço de clima.
        /// </summary>
        public ClimateController(IClimateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Consulta a Open-Meteo e salva o clima atual no banco.
        /// </summary>
        /// <param name="request">Cidade e coordenadas (latitude/longitude).</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>Registro climático recém-salvo.</returns>
        /// <response code="200">Clima sincronizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPost("sync")]
        [ProducesResponseType(typeof(ApiResponseDtos<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Sync([FromBody] ClimateSyncRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDtos<string>.Fail("Dados inválidos."));

            var rec = await _service.FetchAndSaveAsync(request, ct);
            return Ok(ApiResponseDtos<object>.Ok(rec, "Sincronizado com sucesso."));
        }

        /// <summary>
        /// Retorna o histórico mais recente de registros climáticos.
        /// </summary>
        /// <returns>Lista de registros persistidos.</returns>
        /// <response code="200">Lista de registros encontrada.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet("history")]
        [ProducesResponseType(typeof(ApiResponseDtos<IEnumerable<object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> History()
        {
            var list = await _service.GetHistoryAsync();
            return Ok(ApiResponseDtos<object>.Ok(list, "Histórico retornado com sucesso."));
        }
    }
}
