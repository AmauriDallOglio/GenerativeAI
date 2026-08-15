using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class BaixarAudioYoutubeRequest : IRequest<ResultadoOperacao<object>>
    {
        public string Url { get; set; } = string.Empty;
    }
}
