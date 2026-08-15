using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class TranscricaoAudioWhisperHandler : IContratoBaseHandler<TranscricaoAudioWhisperRequest, ResultadoOperacao<object>>
    {
        private readonly IWhisperIntegracaoServico _integracaoAplicacaoServico;

        public TranscricaoAudioWhisperHandler(IWhisperIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(TranscricaoAudioWhisperRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Arquivo is null || request.Arquivo.Length == 0)
            {
                return ResultadoOperacao<object>.GerarErro("Nenhum arquivo foi enviado.", StatusCodes.Status400BadRequest);
            }

            var resultado = await _integracaoAplicacaoServico.TranscricaoAudioAsync(request.Arquivo, cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
