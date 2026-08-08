using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ImportarDocumentoRagHandler : IContratoBaseHandler<ImportarDocumentoRagRequest, ResultadoOperacao<object>>
    {
        private readonly IRagIntegracaoServico _integracaoAplicacaoServico;

        public ImportarDocumentoRagHandler(IRagIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(ImportarDocumentoRagRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Arquivo is null || request.Arquivo.Length == 0)
            {
                return ResultadoOperacao<object>.GerarErro("Arquivo inválido para importação no RAG.", StatusCodes.Status400BadRequest);
            }


            var resultado = await _integracaoAplicacaoServico.ImportarDocumentoAsync(request.Arquivo, cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
