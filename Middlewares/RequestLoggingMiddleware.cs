using System.Diagnostics;

namespace API.Middlewares
{
    /// <summary>
    /// Registra logs estruturados de cada requisição/resposta.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var logMessage = $"[{context.Request.Method}] {context.Request.Path} " +
                             $"=> {context.Response.StatusCode} " +
                             $"({stopwatch.ElapsedMilliseconds} ms)";

            _logger.LogInformation(logMessage);
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
