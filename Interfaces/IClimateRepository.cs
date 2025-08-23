using API.Models;

namespace API.Interfaces
{
    public interface IClimateRepository
    {
        Task AddAsync(ClimateRecord record);
        Task<List<ClimateRecord>> GetAllAsync();
    }
}
