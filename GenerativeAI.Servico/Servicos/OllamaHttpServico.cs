using GenerativeAI.Servico.Dto;
using GenerativeAI.Servico.Util;
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

        public async Task<Resultado<string>> PerguntarAsync(string prompt)
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

            try
            {
                _logger.LogInformation("Enviando pergunta para Ollama: {Prompt}", prompt);
                var response = await _httpClient.PostAsync($"{_OllamaAppSettingsDto.BaseUrl}/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Erro ao comunicar com Ollama: {Erro}", erro);
                    return Resultado<string>.Falha($"Erro ao comunicar com o Ollama: {erro}");
                }

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var resposta = doc.RootElement.TryGetProperty("response", out var respostaJson)
                          ? respostaJson.GetString() ?? string.Empty
                          : json;
                return Resultado<string>.Ok(resposta, "Resposta obtida com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao logar a pergunta para Ollama");
                return Resultado<string>.Falha($"Erro ao comunicar com o Ollama: {ex.Message}");
            }
            


        }
    }
}
