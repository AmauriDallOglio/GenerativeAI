using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Servico.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntegracaoController : ControllerBase
    {
        private readonly RagServico _RagServico;
        private readonly MLNetServico _MLNetServico;

        public IntegracaoController(RagServico ragServico, MLNetServico mLNetServico)
        {
            _RagServico = ragServico;
            _MLNetServico = mLNetServico;
        }

        [HttpGet("Rag/ObterTodos")]
        public async Task<IActionResult> ConsultarRag([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var resultado = await _RagServico.ObterTodosAsync(page, pageSize, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpPost("Rag/ImportarDocumento")]
        public async Task<IActionResult> ImportarDocumento([FromForm] UploadDocumentoRequest request, CancellationToken cancellationToken = default)
        {
            var resultado = await _RagServico.ImportarDocumentoAsync(request?.Arquivo, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/GerarTreinamento")]
        public async Task<IActionResult> ConsultarMLNet(CancellationToken cancellationToken = default)
        {
            var resultado = await _MLNetServico.GerarTreinamentoAsync(cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/ObterTreinamento")]
        public async Task<IActionResult> ObterTreinamento(CancellationToken cancellationToken = default)
        {
            var resultado = await _MLNetServico.ObterTreinamentoAsync(cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/ObterRespostaTreinamento")]
        public async Task<IActionResult> ObterRespostaTreinamento(CancellationToken cancellationToken = default)
        {
            var resultado = await _MLNetServico.ObterRespostaTreinamentoAsync(cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/AtualizarTreinamento")]
        public async Task<IActionResult> AtualizarTreinamento(CancellationToken cancellationToken = default)
        {
            var resultado = await _MLNetServico.AtualizarTreinamentoAsync(cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/Sessoes/ObterTodos")]
        public async Task<IActionResult> ObterSessoes(CancellationToken cancellationToken = default)
        {
            var resultado = await _MLNetServico.ObterSessoesAsync(cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }
    }
}
