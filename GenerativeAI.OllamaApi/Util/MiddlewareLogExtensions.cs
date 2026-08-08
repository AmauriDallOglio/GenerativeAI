namespace GenerativeAI.OllamaApi.Util
{
    public static class MiddlewareLogExtensions
    {
        public static IApplicationBuilder UseRegistroMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RegistroMiddleware>();
        }
    }
}
