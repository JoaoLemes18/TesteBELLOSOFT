using API.Dtos;
using API.Interfaces;
using API.Models;
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
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _authService.RegisterAsync(user.Nome, user.Email, user.Senha);

            if (!result.Success)
                return BadRequest(ApiResponse<string>.Fail(result.Message));

            return Ok(ApiResponse<string>.Ok(null, result.Message));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var result = await _authService.LoginAsync(login.Email, login.Senha);

            if (!result.Success)
                return Unauthorized(ApiResponse<string>.Fail(result.Message));

            return Ok(ApiResponse<object>.Ok(new { token = result.Token, nome = login.Nome, email = login.Email }, "Login realizado com sucesso."));
        }
    }
}
