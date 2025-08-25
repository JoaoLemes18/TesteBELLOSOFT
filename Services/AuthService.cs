using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        public async Task<AuthResultDtos> RegisterAsync(string nome, string email, string senha)
        {
            email = email.Trim();

            var exists = await _users.ExistsByEmailAsync(email);
            if (exists)
            {
                return new AuthResultDtos
                {
                    Success = false,
                    Message = "Email já cadastrado."
                };
            }

            var senhaHash = PasswordHelper.HashPassword(senha);

            var user = new User
            {
                Nome = nome,
                Email = email,
                Senha = senhaHash
            };

            await _users.AddAsync(user);

            return new AuthResultDtos
            {
                Success = true,
                Message = "Usuário registrado com sucesso."
            };
        }

        public async Task<AuthResultDtos> LoginAsync(string email, string senha)
        {
            email = email.Trim();

            var user = await _users.GetByEmailAsync(email);
            if (user == null || !PasswordHelper.VerifyPassword(senha, user.Senha))
            {
                return new AuthResultDtos { Success = false, Message = "Credenciais inválidas" };
            }

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
