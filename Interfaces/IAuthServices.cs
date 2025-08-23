namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(string nome, string email, string senha);
        Task<(bool Success, string Token, string? Message)> LoginAsync(string email, string senha);
    }
}
