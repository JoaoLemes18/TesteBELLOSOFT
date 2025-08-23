using API.Data;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<AuthResultDtos> RegisterAsync(string nome, string email, string senha)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            if (exists)
            {
                return new AuthResultDtos
                {
                    Success = false,
                    Message = "Email já cadastrado."
                };
            }

            var user = new User
            {
                Nome = nome,
                Email = email,
                Senha = senha
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthResultDtos
            {
                Success = true,
                Message = "Usuário registrado com sucesso."
            };
        }


        public async Task<AuthResultDtos> LoginAsync(string email, string senha)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (user == null)
                return new AuthResultDtos { Success = false, Message = "Credenciais inválidas" };

            var token = JwtTokenHelper.GenerateToken(user, _config);

            return new AuthResultDtos
            {
                Success = true,
                Token = token,
                Nome = user.Nome,
                Email = user.Email,
                Message = "Login realizado com sucesso."
            };
        }

    }
}

