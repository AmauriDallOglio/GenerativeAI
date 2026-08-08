using GenerativeAI.Aplicacao.Util;
using System.Diagnostics;

namespace GenerativeAI.Api.Middleware
{
    public  class RegistroRequisicaoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RegistroRequisicaoMiddleware> _logger;
        private readonly IPrintaConsole<RegistroRequisicaoMiddleware> _printaConsole;

        public RegistroRequisicaoMiddleware(RequestDelegate next, ILogger<RegistroRequisicaoMiddleware> logger, IPrintaConsole<RegistroRequisicaoMiddleware> printaConsole)
        {
            _next = next;
            _logger = logger;
            _printaConsole = printaConsole;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cronometro = Stopwatch.StartNew();
            await _next(context);
            cronometro.Stop();

            _printaConsole.Sucesso($"--> {context.Request.Method} {context.Request.Path}{context.Request.QueryString} respondeu {context.Response.StatusCode} em {cronometro.ElapsedMilliseconds}ms");

            //_logger.LogInformation(
            //    "--> {Method} {Path}{QueryString} respondeu {StatusCode} em {ElapsedMilliseconds}ms",
            //    context.Request.Method,
            //    context.Request.Path,
            //    context.Request.QueryString,
            //    context.Response.StatusCode,
            //    cronometro.ElapsedMilliseconds);
        }
    }
    
}
