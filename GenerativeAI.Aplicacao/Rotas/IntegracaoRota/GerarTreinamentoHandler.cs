using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class GerarTreinamentoHandler : IContratoBaseHandler<GerarTreinamentoRequest, ResultadoOperacao<object>>
    {
        private readonly IMLNetIntegracaoServico _integracaoAplicacaoServico;

        public GerarTreinamentoHandler(IMLNetIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(GerarTreinamentoRequest request, CancellationToken cancellationToken = default)
        {
            var resultado = await _integracaoAplicacaoServico.GerarTreinamentoAsync(cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
