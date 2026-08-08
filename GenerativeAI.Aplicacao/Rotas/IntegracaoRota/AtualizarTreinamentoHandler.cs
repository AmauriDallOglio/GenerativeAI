using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class AtualizarTreinamentoHandler : IContratoBaseHandler<AtualizarTreinamentoRequest, ResultadoOperacao<object>>
    {
        private readonly IMLNetIntegracaoServico _integracaoAplicacaoServico;

        public AtualizarTreinamentoHandler(IMLNetIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(AtualizarTreinamentoRequest request, CancellationToken cancellationToken = default)
        {
            var resultado = await _integracaoAplicacaoServico.AtualizarTreinamentoAsync(cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
