using System.Diagnostics;

namespace GenerativeAI.Api.Middleware
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
            var cronometro = Stopwatch.StartNew();
            await _next(context);
            cronometro.Stop();

            context.Response.Headers["X-Response-Time-ms"] = cronometro.ElapsedMilliseconds.ToString();
        }
    }
}