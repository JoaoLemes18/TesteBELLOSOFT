using API.Dtos;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Contrato de autenticação: cadastro e login de usuários.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>Realiza o cadastro de um novo usuário.</summary>
        Task<AuthResultDtos> RegisterAsync(string nome, string email, string senha);

       /// <summary>Autentica o usuário e retorna token + dados básicos.</summary>
       Task<AuthResultDtos> LoginAsync(string email, string senha);
    }
}
