namespace GenerativeAI.OllamaApi.Util
{
    public static class MiddlewareLogExtensions
    {
        public static IApplicationBuilder UseMiddlewareLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareLog>();
        }
    }
}
