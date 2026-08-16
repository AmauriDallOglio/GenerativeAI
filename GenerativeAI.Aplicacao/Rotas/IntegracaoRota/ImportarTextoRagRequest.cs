using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ImportarTextoRagRequest : IRequest<ResultadoOperacao<object>>
    {
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
    }
}
