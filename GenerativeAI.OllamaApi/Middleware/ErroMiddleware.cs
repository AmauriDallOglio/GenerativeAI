using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.OllamaApi.Middleware
{
    public class ErroMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErroMiddleware> _logger;

        public ErroMiddleware(RequestDelegate next, ILogger<ErroMiddleware> logger)
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
                _logger.LogError(ex, "Erro não tratado na aplicação");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var resultado = ResultadoOperacao<object>.GerarErro("Ocorreu um erro interno no servidor.", StatusCodes.Status500InternalServerError);
                await context.Response.WriteAsJsonAsync(resultado);
            }
        }
    }
}
