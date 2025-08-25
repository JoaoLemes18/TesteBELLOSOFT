using BCrypt.Net;

namespace API.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Gera o hash da senha com salt embutido.
        /// </summary>
        public static string HashPassword(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        /// <summary>
        /// Verifica se a senha informada confere com o hash armazenado.
        /// </summary>
        public static bool VerifyPassword(string senha, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
}
