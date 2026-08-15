using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GenerativeAI.Servico.Servicos
{
    public class YoutubeIntegracao : IYoutubeIntegracaoServico
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public YoutubeIntegracao(HttpClient httpClient, IOptions<AppSettingsDto> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.Youtube.ApiKey;
        }

        public async Task<ResultadoOperacao<object>> BaixarAudioAsync(string url, CancellationToken cancellationToken)
        {
            var body = JsonSerializer.Serialize(new { Url = url });
            using var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await Executar(HttpMethod.Post, "api/youtube/BaixarAudio", content, cancellationToken);
            return await BuildResultadoAsync(response, "Solicitação enviada ao Youtube com sucesso.", cancellationToken);
        }

        private async Task<ResultadoOperacao<object>> BuildResultadoAsync(HttpResponseMessage response, string mensagemSucesso, CancellationToken cancellationToken)
        {
            var conteudo = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return ResultadoOperacao<object>.GerarSucesso(conteudo, mensagemSucesso);
            }

            return ResultadoOperacao<object>.GerarErro($"Falha ao consultar o Youtube. Status: {(int)response.StatusCode}. {conteudo}", (int)response.StatusCode);
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
