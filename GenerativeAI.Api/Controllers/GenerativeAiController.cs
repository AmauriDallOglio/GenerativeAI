using GenerativeAI.Aplicacao.Rotas.GenerativeAiRota;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerativeAiController : ControllerBase
    {
        private readonly IYoutubeIntegracaoServico _youtubeServico;
        private readonly IWhisperIntegracaoServico _whisperServico;
        private readonly IRagIntegracaoServico _ragServico;

        public GenerativeAiController(
            IYoutubeIntegracaoServico youtubeServico,
            IWhisperIntegracaoServico whisperServico,
            IRagIntegracaoServico ragServico)
        {
            _youtubeServico = youtubeServico;
            _whisperServico = whisperServico;
            _ragServico = ragServico;
        }

        [HttpPost("TreinamentoYoutube")]
        public async Task<IActionResult> TreinamentoYoutube([FromBody] TreinamentoYoutubeRequest request, CancellationToken cancellationToken = default)
        {
            var handler = new TreinamentoYoutubeHandler(_youtubeServico, _whisperServico, _ragServico);
            ResultadoOperacao<object> resultado = await handler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }
    }
}
