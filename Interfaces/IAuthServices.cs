using API.Dtos;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDtos> RegisterAsync(string nome, string email, string senha);
        Task<AuthResultDtos> LoginAsync(string email, string senha);
    }
}
