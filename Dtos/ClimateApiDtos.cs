using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ClimateApiResponse
    {
        [JsonPropertyName("latitude")] public double Latitude { get; set; }
        [JsonPropertyName("longitude")] public double Longitude { get; set; }

        [JsonPropertyName("current_weather")]
        public ClimateApiCurrentWeather? CurrentWeather { get; set; }
    }

    public class ClimateApiCurrentWeather
    {
        [JsonPropertyName("temperature")] public double Temperature { get; set; }
        [JsonPropertyName("windspeed")] public double WindSpeed { get; set; }
        [JsonPropertyName("winddirection")] public double WindDirection { get; set; }
        [JsonPropertyName("is_day")] public int IsDay { get; set; }
        [JsonPropertyName("time")] public string? TimeIso { get; set; }
    }

    public class ClimateSyncRequest
    {
        [Required(ErrorMessage = "A cidade é obrigatória")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome da cidade deve ter entre 2 e 100 caracteres")]
        public string City { get; set; } = "Cuiabá";

        [Range(-90, 90, ErrorMessage = "Latitude deve estar entre -90 e 90 graus")]
        public double Latitude { get; set; } = -15.60;

        [Range(-180, 180, ErrorMessage = "Longitude deve estar entre -180 e 180 graus")]
        public double Longitude { get; set; } = -56.10;
    }
}
