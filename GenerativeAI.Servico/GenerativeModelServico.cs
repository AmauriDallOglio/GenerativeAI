using Microsoft.Extensions.Configuration;

namespace GenerativeAI.Servico
{
    public class GenerativeModelServico
    {
        private readonly IConfiguration _configuration;

        public GenerativeModelServico(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObterChave()
        {
            var apiKey = _configuration["Gemini:ApiKey"]?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("ERRO FATAL: A chave da API Gemini não foi configurada. Defina Gemini:ApiKey no appsettings ou nas variáveis de ambiente.");
                return string.Empty;
            }

            Console.WriteLine("Chave da API Gemini carregada com sucesso.");
            return apiKey;
        }

        public GenerativeModel Obter()
        {
            var apiKey = ObterChave();
            return new GenerativeModel(apiKey: apiKey, model: "gemini-2.5-flash");
        }
    }
}
