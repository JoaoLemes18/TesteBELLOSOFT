using API.Dtos;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string nome, string email, string senha);
        Task<AuthResult> LoginAsync(string email, string senha);
    }
}
