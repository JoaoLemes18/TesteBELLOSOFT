namespace API.Models
{
    public class ClimateRecord
    {
        public int Id { get; set; }
        public DateTime CapturedAtUtc { get; set; }
        public string City { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double TemperatureC { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public bool IsDay { get; set; }
    }
}
