using GenerativeAI.Servico.Dto;
using GenerativeAI.Servico.Prompt;
using GenerativeAI.Servico;
using GenerativeAI.Servico.Servicos;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.OllamaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OllamaController : ControllerBase
    {

        private readonly OllamaHttpServico _OllamaServico;

        public OllamaController(OllamaHttpServico ollamaServico)
        {
            _OllamaServico = ollamaServico;
        }

        [HttpGet("pergunta")]
        public async Task<IActionResult> Perguntar([FromQuery] string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return BadRequest(new { erro = "Informe uma pergunta válida." });

            var resposta = await _OllamaServico.PerguntarAsync(texto);
            return Ok(new
            {
                pergunta = texto,
                resposta
            });
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
 
            string response = await _OllamaServico.PerguntarAsync(texto);


            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(response, "application/json");

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
            string response = await _OllamaServico.PerguntarAsync(texto);

 
            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(response, "application/json");

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
