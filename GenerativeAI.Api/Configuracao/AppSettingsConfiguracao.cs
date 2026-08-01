using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Servico;
using GenerativeAI.Servico.Servicos;

namespace GenerativeAI.Api.Configuracao
{
    public static class AppSettingsConfiguracao
    {
        public static void Carregar(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettingsDto appSettingsDto = configuration.GetSection("IntegracaoServicos").Get<AppSettingsDto>() ?? new AppSettingsDto();
            services.AddSingleton(appSettingsDto);

            services.Configure<AppSettingsDto>(configuration.GetSection("IntegracaoServicos"));

            services.AddSingleton<GenerativeModelServico>();
            services.AddSingleton(sp =>
            {
                var servico = sp.GetRequiredService<GenerativeModelServico>();
                var apiKey = servico.ObterChave();

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new InvalidOperationException("A chave da API Gemini não foi carregada. Defina Gemini:ApiKey no appsettings ou nas variáveis de ambiente.");
                }

                return new GenerativeModel(apiKey: apiKey, model: "gemini-2.5-flash");
            });

            services.AddHttpClient<RagIntegracao>((sp, client) =>
            {
                var appSettingsDto = sp.GetRequiredService<AppSettingsDto>();
                client.BaseAddress = new Uri(appSettingsDto.Rag.Url ?? "https://localhost:7001");
            });

            services.AddHttpClient<MlNetIntegracao>((sp, client) =>
            {
                var appSettingsDto = sp.GetRequiredService<AppSettingsDto>();
                client.BaseAddress = new Uri(appSettingsDto.MLNet.Url ?? "https://localhost:7002");
            });
        }
    }
}
