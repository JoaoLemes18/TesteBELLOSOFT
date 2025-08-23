using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var result = await _authService.RegisterAsync(req.Nome, req.Email, req.Senha);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.Fail(result.Message));

            return Ok(ApiResponse<string>.Ok(null, result.Message));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _authService.LoginAsync(req.Email, req.Senha);

            if (!result.Success || result.Token is null)
                return Unauthorized(ApiResponse<string>.Fail(result.Message));

            var response = new LoginResponse
            {
                Token = result.Token,
                Nome = result.Nome ?? string.Empty,
                Email = result.Email ?? string.Empty
            };

            return Ok(ApiResponse<LoginResponse>.Ok(response, "Login realizado com sucesso."));
        }
    }
}
