using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Util;
using GenerativeAI.Servico;
using GenerativeAI.Servico.Dto;
using GenerativeAI.Servico.Prompt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GenerativeAI.OllamaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OllamaController : ControllerBase
    {
        private readonly ILogger<OllamaController> _logger;
        private readonly OllamaAplicacaoServico _ollamaAplicacaoServico;

        public OllamaController(OllamaAplicacaoServico ollamaAplicacaoServico, ILogger<OllamaController> logger)
        {
            _ollamaAplicacaoServico = ollamaAplicacaoServico;
            _logger = logger;
        }

        [HttpGet("pergunta")]
        public async Task<IActionResult> Perguntar([FromQuery] string texto)
        {
            _logger.LogInformation("[Middleware iniciado]");
            _logger.LogWarning("[Middleware iniciou com um warning]");
            _logger.LogError("[Middleware iniciou com erro simulado]");

            var resultado = await _ollamaAplicacaoServico.PerguntarAsync(texto);

            if (!resultado.Sucesso)
                return StatusCode(500, resultado);

            return Ok(resultado);



        }



        [HttpPost("EspecialistaOrdemServico")]
        public async Task<IActionResult> EspecialistaOrdemServico([FromBody] ManutentorDto manutentorDto)
        {
            if (manutentorDto == null || string.IsNullOrWhiteSpace(manutentorDto.Nome))
                return BadRequest(new { erro = "O nome do manutentor não pode ser vazio." });



            var lista = new OrdemServicoFactory().GerarListaOrdensServico(manutentorDto.Nome, "Manutentor 2");
            var prompt = new OrdemServicoFactory().ConverterParaTexto(lista);

            PromptDto promptDto = new PromptEngineering().PromptOrdemServico(prompt, "Amauri");

            string texto = promptDto.FormataToString();

            var resultado = await _ollamaAplicacaoServico.PerguntarAsync(texto);

            if (!resultado.Sucesso)
                return StatusCode(500, resultado);

            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(resultado.Mensagem, "application/json");

            //var jsonResponse = Ok(new
            //{
            //    manutentor = manutentorDto.Nome,
            //    prompt = texto,
            //    resposta = response
            //});

            //// Converte para JSON e retorna com Content
            //string jsonString = System.Text.Json.JsonSerializer.Serialize(jsonResponse, new System.Text.Json.JsonSerializerOptions
            //{
            //    WriteIndented = true // Formata com identação
            //});

            //return Content(jsonString, "application/json");

        }


        [HttpPost("EspecialistaOrdemServicoHtml")]
        public async Task<IActionResult> EspecialistaOrdemServicoHtml([FromBody] ManutentorDto manutentorDto)
        {
            if (manutentorDto == null || string.IsNullOrWhiteSpace(manutentorDto.Nome))
                return BadRequest(new { erro = "O nome do manutentor não pode ser vazio." });



            var lista = new OrdemServicoFactory().GerarListaOrdensServico(manutentorDto.Nome, "Manutentor 2");
            var prompt = new OrdemServicoFactory().ConverterParaTexto(lista);

            PromptDto promptDto = new PromptEngineering().PromptOrdemServicoHtml(prompt, "Amauri");

            String texto = promptDto.FormataToString();
            var resultado = await _ollamaAplicacaoServico.PerguntarAsync(texto);

            if (!resultado.Sucesso)
                return StatusCode(500, resultado);


            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(resultado.Mensagem, "application/json");

            //var jsonResponse = Ok(new
            //{
            //    manutentor = manutentorDto.Nome,
            //    prompt = texto,
            //    resposta = response
            //});

            //// Converte para JSON e retorna com Content
            //string jsonString = System.Text.Json.JsonSerializer.Serialize(jsonResponse, new System.Text.Json.JsonSerializerOptions
            //{
            //    WriteIndented = true // Formata com identação
            //});

            //return Content(jsonString, "application/json");

        }



    }
}
