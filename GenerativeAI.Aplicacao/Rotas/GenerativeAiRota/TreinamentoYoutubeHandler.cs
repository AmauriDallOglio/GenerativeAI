using System.Text.Json;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.GenerativeAiRota
{
    public class TreinamentoYoutubeHandler : IContratoBaseHandler<TreinamentoYoutubeRequest, ResultadoOperacao<object>>
    {
        private readonly IYoutubeIntegracaoServico _youtubeServico;
        private readonly IWhisperIntegracaoServico _whisperServico;
        private readonly IRagIntegracaoServico _ragServico;

        public TreinamentoYoutubeHandler(
            IYoutubeIntegracaoServico youtubeServico,
            IWhisperIntegracaoServico whisperServico,
            IRagIntegracaoServico ragServico)
        {
            _youtubeServico = youtubeServico;
            _whisperServico = whisperServico;
            _ragServico = ragServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(TreinamentoYoutubeRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                return ResultadoOperacao<object>.GerarErro("Informe a URL do vídeo.", StatusCodes.Status400BadRequest);
            }

            var download = await _youtubeServico.BaixarAudioAsync(request.Url, cancellationToken);
            if (!download.Sucesso)
            {
                return ResultadoOperacao<object>.GerarErro($"Falha ao baixar áudio do Youtube: {download.Mensagem}", download.StatusCodigo, download.Resultado);
            }

            var caminhoArquivo = ExtrairString(download.Resultado, "audio", "caminhoArquivo")
                ?? ExtrairString(download.Resultado, "resultado", "audio", "caminhoArquivo");
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                return ResultadoOperacao<object>.GerarErro("Download concluído, mas o caminho do arquivo de áudio não foi retornado.", StatusCodes.Status502BadGateway, download.Resultado);
            }

            var transcricao = await _whisperServico.TranscricaoAudioAsync(caminhoArquivo, cancellationToken);
            if (!transcricao.Sucesso)
            {
                return ResultadoOperacao<object>.GerarErro($"Falha ao transcrever áudio no Whisper: {transcricao.Mensagem}", transcricao.StatusCodigo, transcricao.Resultado);
            }

            var texto = ExtrairString(transcricao.Resultado, "resultado", "texto")
                ?? ExtrairString(transcricao.Resultado, "texto");
            if (string.IsNullOrWhiteSpace(texto))
            {
                return ResultadoOperacao<object>.GerarErro("Transcrição concluída, mas nenhum texto foi retornado pelo Whisper.", StatusCodes.Status502BadGateway, transcricao.Resultado);
            }

            var titulo = string.IsNullOrWhiteSpace(request.Titulo)
                ? Path.GetFileNameWithoutExtension(caminhoArquivo)
                : request.Titulo.Trim();

            var importacao = await _ragServico.ImportarTextoAsync(titulo, texto, cancellationToken);
            if (!importacao.Sucesso)
            {
                return ResultadoOperacao<object>.GerarErro($"Falha ao importar transcrição no RAG: {importacao.Mensagem}", importacao.StatusCodigo, importacao.Resultado);
            }

            return ResultadoOperacao<object>.GerarSucesso(new
            {
                Url = request.Url,
                Titulo = titulo,
                CaminhoArquivo = caminhoArquivo,
                Download = download.Resultado,
                Transcricao = transcricao.Resultado,
                Rag = importacao.Resultado
            }, "Treinamento do Youtube concluído com sucesso.");
        }

        private static string? ExtrairString(object? origem, params string[] caminho)
        {
            if (origem is null)
            {
                return null;
            }

            try
            {
                using var documento = JsonDocument.Parse(origem.ToString() ?? string.Empty);
                JsonElement atual = documento.RootElement;

                foreach (var parte in caminho)
                {
                    if (atual.ValueKind != JsonValueKind.Object || !TryGetPropertyIgnoreCase(atual, parte, out atual))
                    {
                        return null;
                    }
                }

                return atual.ValueKind == JsonValueKind.String ? atual.GetString() : atual.ToString();
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static bool TryGetPropertyIgnoreCase(JsonElement elemento, string nome, out JsonElement valor)
        {
            foreach (var propriedade in elemento.EnumerateObject())
            {
                if (string.Equals(propriedade.Name, nome, StringComparison.OrdinalIgnoreCase))
                {
                    valor = propriedade.Value;
                    return true;
                }
            }

            valor = default;
            return false;
        }
    }
}
