using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;
using API.Dtos;

public class ClimateControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ClimateControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Usa banco em memória para os testes
                services.AddDbContext<API.Data.ApplicationDbContext>(opt =>
                    opt.UseInMemoryDatabase("ClimateTestDb"));
            });
        }).CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var user = new { Nome = "ClimaUser", Email = $"clima_{Guid.NewGuid()}@email.com", Senha = "123456" };
        await _client.PostAsJsonAsync("/api/auth/register", user);

        var login = new { Email = user.Email, Senha = user.Senha };
        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        var result = await response.Content.ReadFromJsonAsync<ApiResponseDtos<LoginResponse>>();
        return result!.Data!.Token;
    }

    [Fact]
    public async Task History_SemToken_DeveRetornar401()
    {
        var response = await _client.GetAsync("/api/climate/history");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Sync_ComTokenValido_DeveRetornar200()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new { City = "Cuiaba", Latitude = -15.6, Longitude = -56.1 };

        var response = await _client.PostAsJsonAsync("/api/climate/sync", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task History_ComTokenValido_DeveRetornar200()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/climate/history");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Sync_SemToken_DeveRetornar401()
    {
        var request = new { City = "SP", Latitude = -23.55, Longitude = -46.63 };
        var response = await _client.PostAsJsonAsync("/api/climate/sync", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Sync_ComDadosInvalidos_DeveRetornar400()
    {
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // latitude inválida (fora do intervalo -90 a 90)
        var request = new { City = "CidadeTeste", Latitude = 200.0, Longitude = -56.1 };

        var response = await _client.PostAsJsonAsync("/api/climate/sync", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

}
