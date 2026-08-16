using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IRagIntegracaoServico
    {
        Task<ResultadoOperacao<object>> ObterTodosAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<ResultadoOperacao<object>> ImportarDocumentoAsync(IFormFile arquivo, CancellationToken cancellationToken);
        Task<ResultadoOperacao<object>> ImportarTextoAsync(string titulo, string texto, CancellationToken cancellationToken);
    }
}
