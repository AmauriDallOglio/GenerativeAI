using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace GenerativeAI.Servico.Servicos
{
    public class RagIntegracao : IRagIntegracaoServico
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RagIntegracao(HttpClient httpClient, IOptions<AppSettingsDto> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.Rag.ApiKey;
        }

        public async Task<ResultadoOperacao<object>> ObterTodosAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var response = await SendAsync(HttpMethod.Get, $"api/Rag/ObterTodos?Page={page}&PageSize={pageSize}", cancellationToken: cancellationToken);
            return await BuildResultadoAsync(response, "Consulta ao RAG enviada com sucesso.", cancellationToken);
        }

        public async Task<ResultadoOperacao<object>> ImportarDocumentoAsync(IFormFile arquivo, CancellationToken cancellationToken)
        {
            await using var stream = arquivo.OpenReadStream();
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(arquivo.ContentType ?? "application/octet-stream");
            content.Add(fileContent, "Arquivo", arquivo.FileName);

            var response = await SendAsync(HttpMethod.Post, "api/Rag/ImportarDocumento", content, cancellationToken);
            return await BuildResultadoAsync(response, "Documento enviado ao RAG com sucesso.", cancellationToken);
        }

        private async Task<ResultadoOperacao<object>> BuildResultadoAsync(HttpResponseMessage response, string mensagemSucesso, CancellationToken cancellationToken)
        {
            var conteudo = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return ResultadoOperacao<object>.GerarSucesso(conteudo, mensagemSucesso);
            }

            return ResultadoOperacao<object>.GerarErro($"Falha ao consultar o RAG. Status: {(int)response.StatusCode}. {conteudo}", (int)response.StatusCode);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod metodo, string url, HttpContent? content = null, CancellationToken cancellationToken = default)
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
