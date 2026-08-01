using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OllamaController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly PromptHandler _PromptHandler;
        private readonly PromptGenerativoHandler _PromptGenerativoHandler;
        private readonly ISessaoMemoriaServico _ISessaoMemoriaServico;
        private readonly PromptGenerativoDadosMocadosHandler _PromptGenerativoDadosMocadosHandler;
        public OllamaController(
            ISessaoMemoriaServico iSessaoMemoriaServico,
            IWebHostEnvironment env,
            PromptHandler promptHandler,
            PromptGenerativoHandler promptGenerativoHandler,
            PromptGenerativoDadosMocadosHandler promptGenerativoDadosMocadosHandler
            )
        {
            _ISessaoMemoriaServico = iSessaoMemoriaServico;
            _env = env;
            _PromptHandler = promptHandler;
            _PromptGenerativoHandler = promptGenerativoHandler;
            _PromptGenerativoDadosMocadosHandler = promptGenerativoDadosMocadosHandler;
        }


        //[Authorize(Policy = "ollama.prompt")]
        [HttpGet("Prompt")]
        public async Task<IActionResult?> Prompt([FromQuery] PromptRequest request, CancellationToken cancellationToken)
        {
            ResultadoOperacao resultado = await _PromptHandler.Executar(request, cancellationToken);

            if (resultado.Sucesso)
                return Ok(resultado.Resultado);
            else
                return BadRequest(resultado.Mensagem);
        }

        //[Authorize(Policy = "ollama.prompt")]
        [HttpGet("PromptGenerativo")]
        public async Task<IActionResult> PromptGenerativo([FromQuery] PromptGenerativoRequest request, CancellationToken cancellationToken)
        {
            ResultadoOperacao resultado = await _PromptGenerativoHandler.Executar(request, cancellationToken);

            if (resultado.Sucesso)
                return Ok(resultado.Resultado);
            else
                return BadRequest(resultado);
        }


        //[Authorize(Policy = "ollama.read")]
        [HttpGet("ObterMemoria")]
        public async Task<IActionResult> ObterMemoria(CancellationToken cancellationToken)
        {
            var logs = await _ISessaoMemoriaServico.ObterTodosAsync(cancellationToken);
            return Ok(logs);
        }


        //[Authorize(Policy = "ollama.prompt")]
        [HttpGet("PromptGenerativoDadosMocados")]
        public async Task<IActionResult> PromptGenerativoDadosMocados([FromQuery] PromptGenerativoDadosMocadosRequest request, CancellationToken cancellationToken)
        {
            //var request = new PromptGenerativoDadosMocadosRequest { Pergunta = "Amauri" };
            ResultadoOperacao resultado = await _PromptGenerativoDadosMocadosHandler.Executar(request, cancellationToken);

            if (resultado.Sucesso)
                return Ok(resultado.Resultado);
            else
                return BadRequest(resultado.Mensagem);
        }

    }
}
