using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.GenerativeAiRota
{
    public class TreinamentoYoutubeRequest : IRequest<ResultadoOperacao<object>>
    {
        public string Url { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
    }
}
