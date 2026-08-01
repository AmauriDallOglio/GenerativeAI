using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterRespostaTreinamentoRequest : IRequest<ResultadoOperacao<object>>
    {
        public string Pergunta { get; set; } = string.Empty;
    }
}
