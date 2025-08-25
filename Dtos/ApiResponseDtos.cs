using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using API.Models;

namespace API.Dtos
{
    /// <summary>
    /// Envelope padrão para respostas de API, padronizando sucesso/erro e mensagem.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto retornado em <see cref="Data"/>.</typeparam>
    public class ApiResponseDtos<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        public ApiResponseDtos() { }

        [JsonConstructor]
        public ApiResponseDtos(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponseDtos<T> Ok(T? data, string message = "Operação concluída com sucesso.")
            => new ApiResponseDtos<T>(true, message, data);

        public static ApiResponseDtos<T> Fail(string message)
            => new ApiResponseDtos<T>(false, message, default);
    }



    public class AuthRegisterOkExample : IExamplesProvider<ApiResponseDtos<string>>
    {
        public ApiResponseDtos<string> GetExamples()
            => ApiResponseDtos<string>.Ok(null, "Usuário registrado com sucesso.");
    }

    public class AuthRegisterBadRequestExample : IExamplesProvider<ApiResponseDtos<string>>
    {
        public ApiResponseDtos<string> GetExamples()
            => ApiResponseDtos<string>.Fail("Erro de validação ou e-mail já cadastrado.");
    }

    public class AuthLoginOkExample : IExamplesProvider<ApiResponseDtos<LoginResponse>>
    {
        public ApiResponseDtos<LoginResponse> GetExamples()
            => ApiResponseDtos<LoginResponse>.Ok(
                new LoginResponse
                {
                    Token = "jwt-token-exemplo",
                    Nome = "João Silva",
                    Email = "joao@email.com"
                },
                "Login realizado com sucesso."
            );
    }

    public class AuthLoginUnauthorizedExample : IExamplesProvider<ApiResponseDtos<string>>
    {
        public ApiResponseDtos<string> GetExamples()
            => ApiResponseDtos<string>.Fail("Usuário não autenticado ou credenciais inválidas.");
    }

    public class ClimateSyncOkExample : IExamplesProvider<ApiResponseDtos<ClimateRecord>>
    {
        public ApiResponseDtos<ClimateRecord> GetExamples()
            => ApiResponseDtos<ClimateRecord>.Ok(
                new ClimateRecord
                {
                    Id = 1,
                    City = "Cuiabá",
                    Latitude = -15.601f,
                    Longitude = -56.097f,
                    TemperatureC = 34.5,
                    CapturedAtUtc = DateTime.UtcNow
                },
                "Clima sincronizado com sucesso."
            );
    }

    public class ClimateSyncBadRequestExample : IExamplesProvider<ApiResponseDtos<string>>
    {
        public ApiResponseDtos<string> GetExamples()
            => ApiResponseDtos<string>.Fail("Dados inválidos: latitude/longitude ausentes.");
    }

    public class ClimateSyncUnauthorizedExample : IExamplesProvider<ApiResponseDtos<string>>
    {
        public ApiResponseDtos<string> GetExamples()
            => ApiResponseDtos<string>.Fail("Usuário não autenticado.");
    }
}
