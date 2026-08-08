namespace GenerativeAI.Api.Middleware
{
    public static class ConfiguracaoMiddleware
    {
 
        public static IApplicationBuilder UseRegistroRequisicao(this IApplicationBuilder builder)
        {
            //Mede o tempo de execução da requisição com Stopwatch.
            //Loga método, path, querystring, status e tempo.
            return builder.UseMiddleware<RegistroRequisicaoMiddleware>();
        }

        public static IApplicationBuilder UseErroMiddleware(this IApplicationBuilder builder)
        {
            //Captura exceções não tratadas e retorna 500 Internal Server Error.
            //Loga o erro com ILogger.
            //Retorna um objeto padronizado (ResultadoOperacao.GerarErro).
            return builder.UseMiddleware<ErroMiddleware>();
        }

        public static IApplicationBuilder UseTempoResposta(this IApplicationBuilder builder)
        {
            //Mede tempo de resposta e adiciona header X-Response-Time-ms.
            return builder.UseMiddleware<TempoRespostaMiddleware>();
        }


        public static IApplicationBuilder ConfigurarMiddlewaresApi(this IApplicationBuilder app)
        {
            //app.UseErroMiddleware();
            app.UseRegistroRequisicao();
            //app.UseTempoResposta();
            return app;
        }
    }
}
