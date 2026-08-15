using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IWhisperIntegracaoServico
    {
        Task<ResultadoOperacao<object>> TranscricaoAudioAsync(IFormFile arquivo, CancellationToken cancellationToken);
    }
}
