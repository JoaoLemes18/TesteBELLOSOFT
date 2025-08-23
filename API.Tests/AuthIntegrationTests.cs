using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Usa banco em memória para os testes
                services.AddDbContext<API.Data.ApplicationDbContext>(opt =>
                    opt.UseInMemoryDatabase("AuthTestDb"));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Register_DeveCadastrarUsuarioRetornar200()
    {
        var emailUnico = $"teste_{Guid.NewGuid()}@email.com";
        var user = new { Nome = "Teste", Email = emailUnico, Senha = "123" };

        var response = await _client.PostAsJsonAsync("/api/auth/register", user);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornar200()
    {
        var user = new { Nome = "LoginUser", Email = "login@email.com", Senha = "123" };
        await _client.PostAsJsonAsync("/api/auth/register", user);

        var login = new { Email = user.Email, Senha = user.Senha };
        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_ComCredenciaisInvalidas_DeveRetornar401()
    {
        var login = new { Email = "naoexiste@email.com", Senha = "errada" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Register_ComEmailDuplicado_DeveRetornar400()
    {
        var email = $"duplicado_{Guid.NewGuid()}@email.com";
        var user = new { Nome = "User1", Email = email, Senha = "123" };

        await _client.PostAsJsonAsync("/api/auth/register", user);
        var response = await _client.PostAsJsonAsync("/api/auth/register", user);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_ComSenhaErrada_DeveRetornar401()
    {
        var email = $"user_{Guid.NewGuid()}@email.com";
        var user = new { Nome = "User2", Email = email, Senha = "123" };
        await _client.PostAsJsonAsync("/api/auth/register", user);

        var login = new { Email = email, Senha = "senhaerrada" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
