using System.Diagnostics;

namespace GenerativeAI.OllamaApi.Middleware
{
    public class RegistroRequisicaoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RegistroRequisicaoMiddleware> _logger;

        public RegistroRequisicaoMiddleware(RequestDelegate next, ILogger<RegistroRequisicaoMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation(
                "{Method} {Path}{QueryString} responded {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
