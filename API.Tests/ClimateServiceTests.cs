using API.Models;
using API.Repositories;
using API.Services;
using API.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class ClimateServiceTests
{
    [Fact]
    public async Task FetchAndSaveAsync_DeveRetornarRegistroQuandoApiResponder()
    {
        // Arrange - resposta fake da API Open-Meteo
        var json = """
        {
          "latitude": 37.56,
          "longitude": 126.97,
          "current_weather": {
            "temperature": 25.5,
            "windspeed": 3.1,
            "winddirection": 200,
            "is_day": 1,
            "time": "2025-08-23T12:00:00Z"
          }
        }
        """;

        var handler = new FakeHttpHandler(json);
        var http = new HttpClient(handler);

        var repoMock = new Mock<ClimateRepository>(null!);
        repoMock.Setup(r => r.AddAsync(It.IsAny<ClimateRecord>()))
                .Returns(Task.CompletedTask);

        var loggerMock = new Mock<ILogger<ClimateService>>();
        var service = new ClimateService(http, repoMock.Object, loggerMock.Object);

        var req = new ClimateSyncRequest { City = "Seoul", Latitude = 37.56, Longitude = 126.97 };

        // Act
        var result = await service.FetchAndSaveAsync(req);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Seoul", result.City);
        Assert.Equal(25.5, result.TemperatureC);
    }

    private class FakeHttpHandler : HttpMessageHandler
    {
        private readonly string _response;
        public FakeHttpHandler(string response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var msg = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_response)
            };
            return Task.FromResult(msg);
        }
    }
}
