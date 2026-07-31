namespace GenerativeAI.Aplicacao.Dto
{
    public class AppSettingsDto
    {
        public ApiConfiguracaoAcessoDto Rag { get; set; } = new();
        public ApiConfiguracaoAcessoDto MLNet { get; set; } = new();
    }

    public class ApiConfiguracaoAcessoDto
    {
        public string Url { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
