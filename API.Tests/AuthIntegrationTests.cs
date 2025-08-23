using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
                // Usa banco em memória só pros testes
                services.AddDbContext<API.Data.ApplicationDbContext>(opt =>
                    opt.UseInMemoryDatabase("TestDb"));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Register_DeveCadastrarUsuarioRetornar200()
    {
        var user = new { nome = "Teste", email = "teste@email.com", senha = "123" };

        var response = await _client.PostAsJsonAsync("/api/auth/register", user);

        Assert.True(response.IsSuccessStatusCode);
    }
}
