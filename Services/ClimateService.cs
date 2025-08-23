using API.Dtos;
using API.Interfaces;
using API.Models;
using System.Net.Http.Json;

namespace API.Services
{
    public class ClimateService : IClimateService
    {
        private readonly HttpClient _http;
        private readonly IClimateRepository _repo;
        private readonly ILogger<ClimateService> _logger;

        public ClimateService(HttpClient http, IClimateRepository repo, ILogger<ClimateService> logger)
        {
            _http = http;
            _repo = repo;
            _logger = logger;
        }

        public async Task<ClimateRecord?> FetchAndSaveAsync(ClimateSyncRequest req, CancellationToken ct = default)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={req.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&longitude={req.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&current_weather=true";

            ClimateApiResponse? dto;
            try
            {
                dto = await _http.GetFromJsonAsync<ClimateApiResponse>(url, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar Open-Meteo");
                throw new InvalidOperationException("Falha ao consultar Open-Meteo.");
            }

            if (dto?.CurrentWeather is null)
                throw new InvalidOperationException("Resposta inválida da Open-Meteo.");

            var rec = new ClimateRecord
            {
                CapturedAtUtc = DateTime.UtcNow,
                City = req.City,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                TemperatureC = dto.CurrentWeather.Temperature,
                WindSpeed = dto.CurrentWeather.WindSpeed,
                WindDirection = dto.CurrentWeather.WindDirection,
                IsDay = dto.CurrentWeather.IsDay == 1
            };

            await _repo.AddAsync(rec);
            return rec;
        }

        public Task<List<ClimateRecord>> GetHistoryAsync() => _repo.GetAllAsync();
    }
}
