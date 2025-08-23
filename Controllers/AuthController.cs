using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Endpoints de autenticação e gestão de usuários (cadastro e login).
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Construtor que injeta o serviço de autenticação.
        /// </summary>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="req">Dados do usuário (nome, email e senha).</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Usuário cadastrado com sucesso.</response>
        /// <response code="400">Erro de validação ou e-mail já cadastrado.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponseDtos<string>.Fail("Dados inválidos: " + string.Join(" | ", errors)));
            }

            var result = await _authService.RegisterAsync(req.Nome, req.Email, req.Senha);

            if (!result.Success)
                return BadRequest(ApiResponseDtos<string>.Fail(result.Message));

            return Ok(ApiResponseDtos<string>.Ok(null, result.Message));
        }

        /// <summary>
        /// Realiza login de um usuário.
        /// </summary>
        /// <param name="req">Credenciais de acesso (email e senha).</param>
        /// <returns>Token JWT e dados do usuário autenticado.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseDtos<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDtos<string>), StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponseDtos<string>.Fail("Dados inválidos: " + string.Join(" | ", errors)));
            }

            var result = await _authService.LoginAsync(req.Email, req.Senha);

            if (!result.Success || result.Token is null)
                return Unauthorized(ApiResponseDtos<string>.Fail(result.Message));

            var response = new LoginResponse
            {
                Token = result.Token,
                Nome = result.Nome ?? string.Empty,
                Email = result.Email ?? string.Empty
            };

            return Ok(ApiResponseDtos<LoginResponse>.Ok(response, "Login realizado com sucesso."));
        }
    }
}
