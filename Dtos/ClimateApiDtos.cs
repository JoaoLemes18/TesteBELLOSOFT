using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    /// <summary>
    /// Resposta principal da Open-Meteo para consulta de clima atual.
    /// </summary>
    public class ClimateApiResponse
    {
        /// <summary>Latitude usada/calculada pela API externa.</summary>
        [JsonPropertyName("latitude")] public double Latitude { get; set; }

        /// <summary>Longitude usada/calculada pela API externa.</summary>
        [JsonPropertyName("longitude")] public double Longitude { get; set; }

        /// <summary>Bloco com as informações de clima atual.</summary>
        [JsonPropertyName("current_weather")]
        public ClimateApiCurrentWeather? CurrentWeather { get; set; }
    }

    /// <summary>
    /// Estrutura de clima atual retornada pela Open-Meteo.
    /// </summary>
    public class ClimateApiCurrentWeather
    {
        /// <summary>Temperatura em graus Celsius.</summary>
        [JsonPropertyName("temperature")] public double Temperature { get; set; }

        /// <summary>Velocidade do vento (m/s ou km/h conforme a API externa).</summary>

        [JsonPropertyName("windspeed")] public double WindSpeed { get; set; }

        /// <summary>Direção do vento em graus.</summary>
        [JsonPropertyName("winddirection")] public double WindDirection { get; set; }
        [JsonPropertyName("is_day")] public int IsDay { get; set; }
        [JsonPropertyName("time")] public string? TimeIso { get; set; }
    }


    /// <summary>
    /// Payload para sincronização de clima (consulta + persistência).
    /// </summary>
    /// <remarks>Usado em <c>POST /api/climate/sync</c>.</remarks>

    public class ClimateSyncRequest
    {
        [Required(ErrorMessage = "A cidade é obrigatória")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome da cidade deve ter entre 2 e 100 caracteres")]

        /// <summary>Nome amigável da cidade (apenas para exibição).</summary>
        public string City { get; set; } = "Cuiabá";

        [Range(-90, 90, ErrorMessage = "Latitude deve estar entre -90 e 90 graus")]

        /// <summary>Latitude do ponto consultado.</summary>

        public double Latitude { get; set; } = -15.60;

        [Range(-180, 180, ErrorMessage = "Longitude deve estar entre -180 e 180 graus")]


        /// <summary>Longitude do ponto consultado.</summary>

        public double Longitude { get; set; } = -56.10;
    }
}
