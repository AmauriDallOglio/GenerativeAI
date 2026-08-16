using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Rotas.IntegracaoRota;
using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntegracaoController : ControllerBase
    {
        private readonly ObterTodosRagHandler _obterTodosRagHandler;
        private readonly ImportarDocumentoRagHandler _importarDocumentoRagHandler;
        private readonly ImportarTextoRagHandler _importarTextoRagHandler;
        private readonly GerarTreinamentoHandler _gerarTreinamentoHandler;
        private readonly ObterTreinamentoHandler _obterTreinamentoHandler;
        private readonly ObterRespostaTreinamentoHandler _obterRespostaTreinamentoHandler;
        private readonly AtualizarTreinamentoHandler _atualizarTreinamentoHandler;
        private readonly ObterSessoesHandler _obterSessoesHandler;
        private readonly BaixarAudioYoutubeHandler _baixarAudioYoutubeHandler;
        private readonly TranscricaoAudioWhisperHandler _transcricaoAudioWhisperHandler;

        public IntegracaoController(
            ObterTodosRagHandler obterTodosRagHandler,
            ImportarDocumentoRagHandler importarDocumentoRagHandler,
            ImportarTextoRagHandler importarTextoRagHandler,
            GerarTreinamentoHandler gerarTreinamentoHandler,
            ObterTreinamentoHandler obterTreinamentoHandler,
            ObterRespostaTreinamentoHandler obterRespostaTreinamentoHandler,
            AtualizarTreinamentoHandler atualizarTreinamentoHandler,
            ObterSessoesHandler obterSessoesHandler,
            BaixarAudioYoutubeHandler baixarAudioYoutubeHandler,
            TranscricaoAudioWhisperHandler transcricaoAudioWhisperHandler)
        {
            _obterTodosRagHandler = obterTodosRagHandler;
            _importarDocumentoRagHandler = importarDocumentoRagHandler;
            _importarTextoRagHandler = importarTextoRagHandler;
            _gerarTreinamentoHandler = gerarTreinamentoHandler;
            _obterTreinamentoHandler = obterTreinamentoHandler;
            _obterRespostaTreinamentoHandler = obterRespostaTreinamentoHandler;
            _atualizarTreinamentoHandler = atualizarTreinamentoHandler;
            _obterSessoesHandler = obterSessoesHandler;
            _baixarAudioYoutubeHandler = baixarAudioYoutubeHandler;
            _transcricaoAudioWhisperHandler = transcricaoAudioWhisperHandler;
        }
        //private readonly IContratoBaseHandler<ObterTodosRagRequest, ResultadoOperacao<object>> _obterTodosRagHandler;
        //private readonly IContratoBaseHandler<ImportarDocumentoRagRequest, ResultadoOperacao<object>> _importarDocumentoRagHandler;
        //private readonly IContratoBaseHandler<GerarTreinamentoRequest, ResultadoOperacao<object>> _gerarTreinamentoHandler;
        //private readonly IContratoBaseHandler<ObterTreinamentoRequest, ResultadoOperacao<object>> _obterTreinamentoHandler;
        //private readonly IContratoBaseHandler<ObterRespostaTreinamentoRequest, ResultadoOperacao<object>> _obterRespostaTreinamentoHandler;
        //private readonly IContratoBaseHandler<AtualizarTreinamentoRequest, ResultadoOperacao<object>> _atualizarTreinamentoHandler;
        //private readonly IContratoBaseHandler<ObterSessoesRequest, ResultadoOperacao<object>> _obterSessoesHandler;

        //public IntegracaoController(
        //    IContratoBaseHandler<ObterTodosRagRequest, ResultadoOperacao<object>> obterTodosRagHandler,
        //    IContratoBaseHandler<ImportarDocumentoRagRequest, ResultadoOperacao<object>> importarDocumentoRagHandler,
        //    IContratoBaseHandler<GerarTreinamentoRequest, ResultadoOperacao<object>> gerarTreinamentoHandler,
        //    IContratoBaseHandler<ObterTreinamentoRequest, ResultadoOperacao<object>> obterTreinamentoHandler,
        //    IContratoBaseHandler<ObterRespostaTreinamentoRequest, ResultadoOperacao<object>> obterRespostaTreinamentoHandler,
        //    IContratoBaseHandler<AtualizarTreinamentoRequest, ResultadoOperacao<object>> atualizarTreinamentoHandler,
        //    IContratoBaseHandler<ObterSessoesRequest, ResultadoOperacao<object>> obterSessoesHandler)
        //{
        //    _obterTodosRagHandler = obterTodosRagHandler;
        //    _importarDocumentoRagHandler = importarDocumentoRagHandler;
        //    _gerarTreinamentoHandler = gerarTreinamentoHandler;
        //    _obterTreinamentoHandler = obterTreinamentoHandler;
        //    _obterRespostaTreinamentoHandler = obterRespostaTreinamentoHandler;
        //    _atualizarTreinamentoHandler = atualizarTreinamentoHandler;
        //    _obterSessoesHandler = obterSessoesHandler;
        //}

        [HttpGet("Rag/ObterTodos")]
        public async Task<IActionResult> ConsultarRag([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var request = new ObterTodosRagRequest { Page = page, PageSize = pageSize };
            ResultadoOperacao<object> resultado = await _obterTodosRagHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpPost("Rag/ImportarDocumento")]
        public async Task<IActionResult> ImportarDocumento([FromForm] UploadDocumentoRequest request, CancellationToken cancellationToken = default)
        {
            var handlerRequest = new ImportarDocumentoRagRequest { Arquivo = request?.Arquivo };
            ResultadoOperacao<object> resultado = await _importarDocumentoRagHandler.Executar(handlerRequest, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpPost("Rag/ImportarTexto")]
        public async Task<IActionResult> ImportarTexto([FromBody] ImportarTextoRagRequest request, CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _importarTextoRagHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/GerarTreinamento")]
        public async Task<IActionResult> ConsultarMLNet(CancellationToken cancellationToken = default)
        {
            var request = new GerarTreinamentoRequest();
            ResultadoOperacao<object> resultado = await _gerarTreinamentoHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/ObterTreinamento")]
        public async Task<IActionResult> ObterTreinamento(CancellationToken cancellationToken = default)
        {
            var request = new ObterTreinamentoRequest();
            ResultadoOperacao<object> resultado = await _obterTreinamentoHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/ObterRespostaTreinamento")]
        public async Task<IActionResult> ObterRespostaTreinamento(CancellationToken cancellationToken = default)
        {
            var request = new ObterRespostaTreinamentoRequest();
            ResultadoOperacao<object> resultado = await _obterRespostaTreinamentoHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/AtualizarTreinamento")]
        public async Task<IActionResult> AtualizarTreinamento(CancellationToken cancellationToken = default)
        {
            var request = new AtualizarTreinamentoRequest();
            ResultadoOperacao<object> resultado = await _atualizarTreinamentoHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpGet("MLNet/Sessoes/ObterTodos")]
        public async Task<IActionResult> ObterSessoes(CancellationToken cancellationToken = default)
        {
            var request = new ObterSessoesRequest();
            ResultadoOperacao<object> resultado = await _obterSessoesHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpPost("Youtube/BaixarAudio")]
        public async Task<IActionResult> BaixarAudioYoutube([FromBody] BaixarAudioYoutubeRequest request, CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await _baixarAudioYoutubeHandler.Executar(request, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

        [HttpPost("Whisper/TranscricaoAudio")]
        public async Task<IActionResult> TranscricaoAudioWhisper([FromForm] UploadDocumentoRequest request, CancellationToken cancellationToken = default)
        {
            var handlerRequest = new TranscricaoAudioWhisperRequest { Arquivo = request?.Arquivo };
            ResultadoOperacao<object> resultado = await _transcricaoAudioWhisperHandler.Executar(handlerRequest, cancellationToken);
            return resultado.Sucesso ? Ok(resultado) : StatusCode(resultado.StatusCodigo ?? StatusCodes.Status502BadGateway, resultado);
        }

    }
}
