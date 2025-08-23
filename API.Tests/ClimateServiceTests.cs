using API.Dtos;
using API.Models;
using API.Services;
using Moq;
using Microsoft.Extensions.Logging;
using System.Net;
using Xunit;
using API.Interfaces;

public class ClimateServiceTests
{
    [Fact]
    public async Task FetchAndSaveAsync_DeveRetornarRegistro()
    {
        // Arrange
        var repoMock = new Mock<IClimateRepository>();
        var loggerMock = new Mock<ILogger<ClimateService>>();

        var fakeHttp = new HttpClient(new FakeHandler())
        {
            BaseAddress = new Uri("http://localhost")
        };

        var service = new ClimateService(fakeHttp, repoMock.Object, loggerMock.Object);

        var request = new ClimateSyncRequest
        {
            City = "TestCity",
            Latitude = 10,
            Longitude = 20
        };

        // Act
        var record = await service.FetchAndSaveAsync(request);

        // Assert
        Assert.NotNull(record);
        Assert.Equal("TestCity", record.City);
    }

    [Fact]
    public async Task GetHistoryAsync_DeveRetornarListaDeRegistros()
    {
        // Arrange
        var repoMock = new Mock<IClimateRepository>();
        var loggerMock = new Mock<ILogger<ClimateService>>();

        var fakeHttp = new HttpClient(new FakeHandler())
        {
            BaseAddress = new Uri("http://localhost")
        };

        // cria lista fake
        var registros = new List<ClimateRecord>
        {
            new ClimateRecord { City = "Cuiaba", TemperatureC = 30, CapturedAtUtc = DateTime.UtcNow },
            new ClimateRecord { City = "Várzea Grande", TemperatureC = 28, CapturedAtUtc = DateTime.UtcNow }
        };

        // Configura mock para retornar a lista
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(registros);

        var service = new ClimateService(fakeHttp, repoMock.Object, loggerMock.Object);

        // Act
        var result = await service.GetHistoryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.City == "Cuiaba");
        Assert.Contains(result, r => r.City == "Várzea Grande");
    }

    private class FakeHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var json = @"{
                ""latitude"": 10,
                ""longitude"": 20,
                ""current_weather"": {
                    ""temperature"": 25.5,
                    ""windspeed"": 10.1,
                    ""winddirection"": 180,
                    ""is_day"": 1
                }
            }";

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            });
        }
    }
}
