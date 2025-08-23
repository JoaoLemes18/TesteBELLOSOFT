using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Endpoints para sincronização e consulta de registros climáticos persistidos.
    /// Requer autenticação via JWT.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClimateController : ControllerBase
    {
        private readonly ClimateService _service;

        /// <summary>
        /// Construtor do controller de Clima.
        /// </summary>
        /// <param name="service">Serviço de regras de negócio de clima.</param>
        public ClimateController(ClimateService service) => _service = service;

        /// <summary>
        /// Sincroniza o clima atual a partir da Open-Meteo e persiste no banco.
        /// </summary>
        /// <param name="request">Cidade e coordenadas (latitude/longitude) a consultar.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <remarks>
        /// Exemplo de corpo:
        ///
        ///     {
        ///       "city": "Seoul",
        ///       "latitude": 37.5665,
        ///       "longitude": 126.9780
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna o registro salvo.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Não autenticado.</response>
        /// <response code="500">Erro interno ao consultar a API externa ou persistir.</response>
        [HttpPost("sync")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Sync([FromBody] ClimateSyncRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rec = await _service.FetchAndSaveAsync(request, ct);
            return Ok(new { message = "Sincronizado com sucesso.", record = rec });
        }

        /// <summary>
        /// Retorna o histórico mais recente de registros climáticos persistidos.
        /// </summary>
        /// <response code="200">Lista de registros.</response>
        /// <response code="401">Não autenticado.</response>
        [HttpGet("history")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> History()
        {
            var list = await _service.GetHistoryAsync();
            return Ok(list);
        }
    }
}
