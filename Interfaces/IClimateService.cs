using API.Dtos;
using API.Models;

namespace API.Interfaces
{

    /// <summary>
    /// Regras de negócio para sincronização e consulta de clima.
    /// </summary>  
    public interface IClimateService
    {

        /// <summary>
        /// Consulta a Open-Meteo com as coordenadas informadas e persiste o resultado.
        /// </summary>
        Task<ClimateRecord?> FetchAndSaveAsync(ClimateSyncRequest req, CancellationToken ct = default);

        /// <summary>Retorna o histórico mais recente de registros persistidos.</summary>
        Task<List<ClimateRecord>> GetHistoryAsync();
    }
}
