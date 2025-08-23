using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    /// <summary>
    /// Endpoints de autenticação e gestão básica de usuários (cadastro e login).
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        /// <summary>
        /// Construtor do controller de autenticação.
        /// </summary>
        /// <param name="context">Contexto de acesso ao banco de dados.</param>
        /// <param name="config">Configurações da aplicação (JWT).</param>
        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <param name="user">Dados do usuário: nome, email e senha.</param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///       "nome": "João",
        ///       "email": "joao@email.com",
        ///       "senha": "123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Usuário cadastrado com sucesso.</response>
        /// <response code="400">Email já cadastrado ou dados inválidos.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_context.Users.Any(u => u.Email == user.Email))
                return BadRequest("Email já cadastrado.");

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Usuário cadastrado com sucesso!" });
        }

        /// <summary>
        /// Realiza login e retorna um token JWT.
        /// </summary>
        /// <param name="login">Credenciais de acesso: email e senha.</param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///       "email": "joao@email.com",
        ///       "senha": "123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Token gerado com sucesso.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] User login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
            if (user is null) return Unauthorized("Credenciais inválidas");

            var token = GenerateJwtToken(user);
            return Ok(new { token, nome = user.Nome, email = user.Email });
        }

        /// <summary>
        /// Gera um token JWT para o usuário autenticado.
        /// </summary>
        /// <param name="user">Usuário autenticado.</param>
        /// <returns>Token JWT como string.</returns>
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("nome", user.Nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
