using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string nome, string email, string senha)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return (false, "Email já cadastrado.");

            var user = new User { Nome = nome, Email = email, Senha = senha };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "Usuário cadastrado com sucesso!");
        }

        public async Task<(bool Success, string Token, string? Message)> LoginAsync(string email, string senha)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (user is null)
                return (false, string.Empty, "Credenciais inválidas");

            var token = GenerateJwtToken(user);
            return (true, token, null);
        }

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
