using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterRespostaTreinamentoHandler : IContratoBaseHandler<ObterRespostaTreinamentoRequest, ResultadoOperacao<object>>
    {
        private readonly IntegracaoAplicacaoServico _integracaoAplicacaoServico;

        public ObterRespostaTreinamentoHandler(IntegracaoAplicacaoServico integracaoAplicacaoServico)
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
