using System.Diagnostics;

namespace GenerativeAI.OllamaApi.Middleware
{
    public class TempoRespostaMiddleware
    {
        private readonly RequestDelegate _next;

        public TempoRespostaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            context.Response.Headers["X-Response-Time-ms"] = stopwatch.ElapsedMilliseconds.ToString();
        }
    }
}
