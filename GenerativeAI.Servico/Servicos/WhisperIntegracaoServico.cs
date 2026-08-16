using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

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
            return await EnviarArquivoAsync(stream, arquivo.FileName, arquivo.ContentType ?? "application/octet-stream", cancellationToken);
        }

        public async Task<ResultadoOperacao<object>> TranscricaoAudioAsync(string caminhoArquivo, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo) || !File.Exists(caminhoArquivo))
            {
                return ResultadoOperacao<object>.GerarErro("Arquivo de áudio não encontrado para transcrição.", StatusCodes.Status404NotFound);
            }

            await using var stream = File.OpenRead(caminhoArquivo);
            return await EnviarArquivoAsync(stream, Path.GetFileName(caminhoArquivo), "audio/mpeg", cancellationToken);
        }

        private async Task<ResultadoOperacao<object>> EnviarArquivoAsync(Stream stream, string nomeArquivo, string contentType, CancellationToken cancellationToken)
        {
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Add(fileContent, "arquivo", nomeArquivo);

            var response = await Executar(HttpMethod.Post, "api/Whisper/TranscricaoAudio", content, cancellationToken);
            return await BuildResultadoAsync(response, "Áudio enviado ao Whisper com sucesso.", cancellationToken);
        }

        private async Task<ResultadoOperacao<object>> BuildResultadoAsync(HttpResponseMessage response, string mensagemSucesso, CancellationToken cancellationToken)
        {
            var conteudo = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                if (OperacaoFalhou(conteudo, out var mensagemErro))
                {
                    return ResultadoOperacao<object>.GerarErro($"Falha ao consultar o Whisper. {mensagemErro}", (int)response.StatusCode, conteudo);
                }

                return ResultadoOperacao<object>.GerarSucesso(conteudo, mensagemSucesso);
            }

            return ResultadoOperacao<object>.GerarErro($"Falha ao consultar o Whisper. Status: {(int)response.StatusCode}. {conteudo}", (int)response.StatusCode);
        }

        private static bool OperacaoFalhou(string conteudo, out string mensagem)
        {
            mensagem = string.Empty;

            try
            {
                using var documento = JsonDocument.Parse(conteudo);
                if (documento.RootElement.TryGetProperty("sucesso", out var sucesso) && sucesso.ValueKind == JsonValueKind.False)
                {
                    mensagem = documento.RootElement.TryGetProperty("mensagem", out var mensagemJson)
                        ? mensagemJson.GetString() ?? string.Empty
                        : string.Empty;
                    return true;
                }
            }
            catch (JsonException)
            {
                return false;
            }

            return false;
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
