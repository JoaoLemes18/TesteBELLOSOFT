using System.Net;
using System.Text.Json;
using API.Dtos;

namespace API.Middlewares
{

    /// <summary>
    /// Captura exceções não tratadas e retorna envelope padronizado.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro capturado pelo middleware");

                var statusCode = HttpStatusCode.InternalServerError;
                var message = "Ocorreu um erro inesperado. Tente novamente.";

                switch (ex)
                {
                    case InvalidOperationException:
                        statusCode = HttpStatusCode.BadRequest;
                        message = ex.Message; 
                        break;

                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Unauthorized;
                        message = "Acesso não autorizado.";
                        break;

                    case KeyNotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        message = "Recurso não encontrado.";
                        break;

                }

                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                var response = ApiResponseDtos<string>.Fail(message);

                if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
                {
                    response.Message += $" (Detalhes: {ex.Message})";
                }

                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
