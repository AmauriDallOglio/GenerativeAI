using GenerativeAI.Servico;
using GenerativeAI.Servico.Dto;
using GenerativeAI.Servico.Prompt;
using GenerativeAI.Servico.Servicos;
using GenerativeAI.Servico.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GenerativeAI.OllamaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OllamaController : ControllerBase
    {
        private readonly ILogger<OllamaController> _logger;
        private readonly OllamaHttpServico _OllamaServico;

        public OllamaController(OllamaHttpServico ollamaServico, ILogger<OllamaController> logger)
        {
            _OllamaServico = ollamaServico;
            _logger = logger;
        }

        [HttpGet("pergunta")]
        public async Task<IActionResult> Perguntar([FromQuery] string texto)
        {
            _logger.LogInformation("[Middleware iniciado]");
            _logger.LogWarning("[Middleware iniciou com um warning]");
            _logger.LogError("[Middleware iniciou com erro simulado]");

            if (string.IsNullOrWhiteSpace(texto))
                return BadRequest(Resultado<string>.Falha("Informe uma pergunta válida."));

            var resultado = await _OllamaServico.PerguntarAsync(texto);

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

            var resultado = await _OllamaServico.PerguntarAsync(texto);

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
            var resultado = await _OllamaServico.PerguntarAsync(texto);

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
