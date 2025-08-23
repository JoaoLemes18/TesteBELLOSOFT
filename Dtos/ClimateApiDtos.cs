using System.Text.Json.Serialization;

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
        public string City { get; set; } = "Cuiabá";
        public double Latitude { get; set; } = -15.60;
        public double Longitude { get; set; } = -56.10;
    }
}
