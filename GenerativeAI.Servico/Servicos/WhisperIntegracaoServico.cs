using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace GenerativeAI.Servico.Servicos
{
    public class WhisperIntegracao : IWhisperIntegracaoServico
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WhisperIntegracao(HttpClient httpClient, IOptions<AppSettingsDto> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.Whisper.ApiKey;
        }

        public async Task<ResultadoOperacao<object>> TranscricaoAudioAsync(IFormFile arquivo, CancellationToken cancellationToken)
        {
            await using var stream = arquivo.OpenReadStream();
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(arquivo.ContentType ?? "application/octet-stream");
            content.Add(fileContent, "arquivo", arquivo.FileName);

            var response = await Executar(HttpMethod.Post, "api/Whisper/TranscricaoAudio", content, cancellationToken);
            return await BuildResultadoAsync(response, "Áudio enviado ao Whisper com sucesso.", cancellationToken);
        }

        private async Task<ResultadoOperacao<object>> BuildResultadoAsync(HttpResponseMessage response, string mensagemSucesso, CancellationToken cancellationToken)
        {
            var conteudo = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return ResultadoOperacao<object>.GerarSucesso(conteudo, mensagemSucesso);
            }

            return ResultadoOperacao<object>.GerarErro($"Falha ao consultar o Whisper. Status: {(int)response.StatusCode}. {conteudo}", (int)response.StatusCode);
        }

        private async Task<HttpResponseMessage> Executar(HttpMethod metodo, string url, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(metodo, url);
            request.Headers.Add("X-Api-Key", _apiKey);
            if (content is not null)
            {
                request.Content = content;
            }

            return await _httpClient.SendAsync(request, cancellationToken);
        }
    }
}
