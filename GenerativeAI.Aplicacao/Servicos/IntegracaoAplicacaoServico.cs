using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Servicos
{
    public class IntegracaoAplicacaoServico
    {
        private readonly IRagIntegracaoServico _ragIntegracaoServico;
        private readonly IMLNetIntegracaoServico _mlNetIntegracaoServico;

        public IntegracaoAplicacaoServico(IRagIntegracaoServico ragIntegracaoServico, IMLNetIntegracaoServico mlNetIntegracaoServico)
        {
            _ragIntegracaoServico = ragIntegracaoServico;
            _mlNetIntegracaoServico = mlNetIntegracaoServico;
        }

        public async Task<ResultadoOperacao<object>> ConsultarRagAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _ragIntegracaoServico.ObterTodosAsync(page, pageSize, cancellationToken);
            return resultado;
        }
            

        public async Task<ResultadoOperacao<object>> ImportarDocumentoAsync(IFormFile? arquivo, CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _ragIntegracaoServico.ImportarDocumentoAsync(arquivo, cancellationToken);
            return resultado;
        }
 

        public async Task<ResultadoOperacao<object>> GerarTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _mlNetIntegracaoServico.GerarTreinamentoAsync(cancellationToken);
            return resultado;
        }

        public async Task<ResultadoOperacao<object>> ObterTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _mlNetIntegracaoServico.ObterTreinamentoAsync(cancellationToken);
            return resultado;
        }

        public async Task<ResultadoOperacao<object>> ObterRespostaTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _mlNetIntegracaoServico.ObterRespostaTreinamentoAsync(cancellationToken);
            return resultado;
        }

        public async Task<ResultadoOperacao<object>> AtualizarTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _mlNetIntegracaoServico.AtualizarTreinamentoAsync(cancellationToken);
            return resultado;
        }   

        public async Task<ResultadoOperacao<object>> ObterSessoesAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _mlNetIntegracaoServico.ObterSessoesAsync(cancellationToken);
            return resultado;
        } 




    }
}
