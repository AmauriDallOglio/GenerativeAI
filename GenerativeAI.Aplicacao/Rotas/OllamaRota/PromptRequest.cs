using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.OllamaRota
{
    public class PromptRequest : IRequest<ResultadoOperacao<object>>
    {

        public string Pergunta { get; set; } = string.Empty;
    }
}
