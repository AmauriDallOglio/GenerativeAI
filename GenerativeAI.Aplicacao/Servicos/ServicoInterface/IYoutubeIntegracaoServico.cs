using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IYoutubeIntegracaoServico
    {
        Task<ResultadoOperacao<object>> BaixarAudioAsync(string url, CancellationToken cancellationToken);
    }
}
