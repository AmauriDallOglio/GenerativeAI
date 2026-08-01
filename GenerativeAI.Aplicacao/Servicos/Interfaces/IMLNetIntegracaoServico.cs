using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IMLNetIntegracaoServico
    {
        Task<ResultadoOperacao<object>> GerarTreinamentoAsync(CancellationToken cancellationToken);
        Task<ResultadoOperacao<object>> ObterTreinamentoAsync(CancellationToken cancellationToken);
        Task<ResultadoOperacao<object>> ObterRespostaTreinamentoAsync(CancellationToken cancellationToken);
        Task<ResultadoOperacao<object>> AtualizarTreinamentoAsync(CancellationToken cancellationToken);
        Task<ResultadoOperacao<object>> ObterSessoesAsync(CancellationToken cancellationToken);
    }
}
