using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class BaixarAudioYoutubeHandler : IContratoBaseHandler<BaixarAudioYoutubeRequest, ResultadoOperacao<object>>
    {
        private readonly IYoutubeIntegracaoServico _integracaoAplicacaoServico;

        public BaixarAudioYoutubeHandler(IYoutubeIntegracaoServico integracaoAplicacaoServico)
        {
            _integracaoAplicacaoServico = integracaoAplicacaoServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(BaixarAudioYoutubeRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                return ResultadoOperacao<object>.GerarErro("Informe a URL do vídeo.", StatusCodes.Status400BadRequest);
            }

            var resultado = await _integracaoAplicacaoServico.BaixarAudioAsync(request.Url, cancellationToken);
            return resultado.Sucesso
                ? ResultadoOperacao<object>.GerarSucesso(resultado.Resultado, resultado.Mensagem)
                : ResultadoOperacao<object>.GerarErro(resultado.Mensagem, resultado.StatusCodigo, resultado.Resultado);
        }
    }
}
