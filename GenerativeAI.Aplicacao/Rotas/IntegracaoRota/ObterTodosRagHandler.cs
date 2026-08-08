using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterTodosRagHandler : IContratoBaseHandler<ObterTodosRagRequest, ResultadoOperacao<object>>
    {
        private readonly IRagIntegracaoServico _integracaoAplicacaoServico;

        public ObterTodosRagHandler(IRagIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(ObterTodosRagRequest request, CancellationToken cancellationToken = default)
        {
            var resultado = await _integracaoAplicacaoServico.ObterTodosAsync(request.Page, request.PageSize, cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
