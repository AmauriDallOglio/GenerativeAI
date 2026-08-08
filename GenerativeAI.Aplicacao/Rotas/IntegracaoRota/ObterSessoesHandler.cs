using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterSessoesHandler : IContratoBaseHandler<ObterSessoesRequest, ResultadoOperacao<object>>
    {
        private readonly IMLNetIntegracaoServico _integracaoAplicacaoServico;

        public ObterSessoesHandler(IMLNetIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(ObterSessoesRequest request, CancellationToken cancellationToken = default)
        {
            var resultado = await _integracaoAplicacaoServico.ObterSessoesAsync(cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
