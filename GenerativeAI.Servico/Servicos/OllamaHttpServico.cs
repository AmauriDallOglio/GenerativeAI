using GenerativeAI.Servico.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GenerativeAI.Servico.Servicos
{
    public class OllamaHttpServico
    {
        private readonly HttpClient _httpClient;
        private readonly OllamaAppSettingsDto _OllamaAppSettingsDto;
        private readonly ILogger<OllamaHttpServico> _logger;
        public OllamaHttpServico(HttpClient httpClient, IOptions<OllamaAppSettingsDto> ollamaAppSettingsDto, ILogger<OllamaHttpServico> logger)
        {
            _httpClient = httpClient;
            _OllamaAppSettingsDto = ollamaAppSettingsDto.Value;
            _logger = logger;
        }

        public async Task<string> PerguntarAsync(string prompt)
        {
            var body = new
            {
                model = _OllamaAppSettingsDto.Modelo,
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_OllamaAppSettingsDto.BaseUrl}/api/generate", content);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                _logger.LogError("Erro ao comunicar com Ollama: {Erro}", erro);
                return $"Erro ao comunicar com o Ollama: {erro}";
            }

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("response", out var resposta)
                ? resposta.GetString() ?? string.Empty
                : json;
        }
    }
}
