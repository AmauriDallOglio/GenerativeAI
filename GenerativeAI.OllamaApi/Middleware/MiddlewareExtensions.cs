using GenerativeAI.OllamaApi.Util;

namespace GenerativeAI.OllamaApi.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErroMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErroMiddleware>();
        }

        public static IApplicationBuilder UseRegistroRequisicao(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RegistroRequisicaoMiddleware>();
        }

        public static IApplicationBuilder UseTempoResposta(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TempoRespostaMiddleware>();
        }

        public static IApplicationBuilder UseMiddlewaresApi(this IApplicationBuilder app)
        {
            app.UseErroMiddleware();
            app.UseRegistroRequisicao();
            app.UseTempoResposta();
            app.UseRegistroMiddleware();
            return app;
        }
    }
}
