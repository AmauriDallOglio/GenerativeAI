using System.Diagnostics;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Api.Middleware
{
    public static class ControleSessaoMiddleware
    {
        public static IApplicationBuilder UseControleSessao(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ControleSessaoMiddlewareImpl>();
        }

        private class ControleSessaoMiddlewareImpl
        {
            private readonly RequestDelegate _next;
            private readonly IPrintaConsole<ControleSessaoMiddlewareImpl> _printaConsole;

            public ControleSessaoMiddlewareImpl(RequestDelegate next, IPrintaConsole<ControleSessaoMiddlewareImpl> printaConsole)
            {
                _next = next;
                _printaConsole = printaConsole;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var cronometro = Stopwatch.StartNew();
                context.Session.SetString("SessionLastAccess", DateTime.UtcNow.ToString("o"));
                var sessionId = context.Session.Id;

                if (!context.Session.TryGetValue("SessionStarted", out _))
                {
                    context.Session.SetString("SessionStarted", DateTime.UtcNow.ToString("o"));
                    context.Session.SetString("SessionCreatedPath", context.Request.Path);
                    _printaConsole.ImprimirSemCor($"Sessão {sessionId} iniciada: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
                }
                else
                {
                    _printaConsole.ImprimirSemCor($"Sessão {sessionId} entrada: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
                }

                var ocorreuErro = false;

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    ocorreuErro = true;
                    _printaConsole.Error($"Sessão {sessionId} encontrou erro: {ex.Message}");
                    throw;
                }
                finally
                {
                    cronometro.Stop();
                    var statusCode = context.Response.StatusCode;
                    var mensagem = $"Sessão {sessionId} saída: {context.Request.Method} {context.Request.Path} - {statusCode} em {cronometro.ElapsedMilliseconds} ms";

                    if (!ocorreuErro && statusCode < 400)
                        _printaConsole.Sucesso(mensagem);
                    else
                        _printaConsole.Error(mensagem);
                }
            }
        }
    }
}
