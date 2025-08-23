using API.Models;

namespace API.Repositories
{
    public interface IClimateRepository
    {
        Task AddAsync(ClimateRecord record);
        Task<List<ClimateRecord>> GetAllAsync();
    }
}
