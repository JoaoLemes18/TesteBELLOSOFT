using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace API.Controllers
{
    /// <summary>
    /// Endpoints de autenticação e gestão de usuários (cadastro e login).
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        [HttpPost("register")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuthRegisterOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AuthRegisterBadRequestExample))]
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
        [HttpPost("login")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuthLoginOkExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(AuthLoginUnauthorizedExample))]
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
