using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterRespostaTreinamentoHandler : IContratoBaseHandler<ObterRespostaTreinamentoRequest, ResultadoOperacao<object>>
    {
        private readonly IMLNetIntegracaoServico _integracaoAplicacaoServico;

        public ObterRespostaTreinamentoHandler(IMLNetIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(ObterRespostaTreinamentoRequest request, CancellationToken cancellationToken = default)
        {
            var resultado = await _integracaoAplicacaoServico.ObterRespostaTreinamentoAsync(request.Pergunta, cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
