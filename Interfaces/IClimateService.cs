using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IClimateService
    {
        Task<ClimateRecord?> FetchAndSaveAsync(ClimateSyncRequest req, CancellationToken ct = default);
        Task<List<ClimateRecord>> GetHistoryAsync();
    }
}
