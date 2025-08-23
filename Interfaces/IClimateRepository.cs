using API.Models;

namespace API.Interfaces
{
    /// <summary>
    /// Acesso a dados referentes a <see cref="ClimateRecord"/>.
    /// </summary>
    public interface IClimateRepository
    {
        /// <summary>Adiciona um novo registro climático e salva no banco.</summary>
        Task AddAsync(ClimateRecord record);

        /// <summary>Retorna a lista dos últimos registros persistidos (limitado).</summary>
        Task<List<ClimateRecord>> GetAllAsync();
    }
}
