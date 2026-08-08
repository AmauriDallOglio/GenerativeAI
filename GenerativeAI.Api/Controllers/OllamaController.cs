using GenerativeAI.Aplicacao.Rotas.OllamaRota;
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
 

        public OllamaController(
            IWebHostEnvironment env,
            PromptHandler promptHandler,
            PromptGenerativoHandler promptGenerativoHandler
            )
        {
            _env = env;
            _PromptHandler = promptHandler;
            _PromptGenerativoHandler = promptGenerativoHandler;
        }



        [HttpPost("Prompt")]
        public async Task<IActionResult> Prompt([FromBody] PromptRequest? request, CancellationToken cancellationToken)
        {
            ResultadoOperacao<object> resultado = await _PromptHandler.Executar(request, cancellationToken);

            if (resultado.Sucesso)
                return Ok(resultado.Resultado ?? resultado);

            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status500InternalServerError,
                new { mensagem = resultado.Mensagem });
        }


        [HttpPost("PromptGenerativo")]
        public async Task<IActionResult> PromptGenerativo([FromBody] PromptGenerativoRequest? request, CancellationToken cancellationToken)
        {
            ResultadoOperacao<object> resultado = await _PromptGenerativoHandler.Executar(request, cancellationToken);

            if (resultado.Sucesso)
                return Ok(resultado.Resultado ?? resultado);

            return StatusCode(resultado.StatusCodigo ?? StatusCodes.Status500InternalServerError,
                new { mensagem = resultado.Mensagem });
        }




    }
}
