namespace GenerativeAI.OllamaApi.Util
{
    public class RegistroMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RegistroMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("RegistroMiddleware");
            _logger.LogInformation("[Middleware foi iniciado]...");
            _logger.LogWarning("[Middleware foi iniciado em warning]...");
            _logger.LogError("[Middleware foi iniciado com erros]...");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("[O Middleware está em execução]...");
            await _next.Invoke(httpContext);

            return;
        }

    }
}
