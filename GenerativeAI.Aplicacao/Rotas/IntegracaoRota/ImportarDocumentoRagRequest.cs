using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ImportarDocumentoRagRequest : IRequest<ResultadoOperacao<object>>
    {
        public IFormFile? Arquivo { get; set; }
    }
}
