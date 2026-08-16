using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ImportarTextoRagHandler : IContratoBaseHandler<ImportarTextoRagRequest, ResultadoOperacao<object>>
    {
        private readonly IRagIntegracaoServico _integracaoAplicacaoServico;

        public ImportarTextoRagHandler(IRagIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(ImportarTextoRagRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Texto))
            {
                return ResultadoOperacao<object>.GerarErro("Nenhum texto foi informado para importação no RAG.", StatusCodes.Status400BadRequest);
            }

            var resultado = await _integracaoAplicacaoServico.ImportarTextoAsync(request.Titulo, request.Texto, cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
