using GenerativeAI.Servico;
using GenerativeAI.Servico.Dto;
using GenerativeAI.Servico.Prompt;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : ControllerBase
    {
        private readonly GenerativeModel _model;


        public GeminiController(GenerativeModel model)
        {
            _model = model;

        }

 
        [HttpPost("PerguntaEmGeral")]
        public async Task<IActionResult> PerguntaEmGeral([FromBody] PerguntaRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Pergunta))
                return BadRequest("A pergunta não pode ser vazia.");

            try
            {
                var response = await _model.GenerateContentAsync(request.Pergunta);
                // return Ok(new { resposta = response.Text() });
                return Content(response.Text, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na chamada da API Gemini: {ex.Message}");
                return StatusCode(500, new { error = "Ocorreu um erro ao se comunicar com a API Gemini.", details = ex.Message });
            }
        }

 
        [HttpPost("EspecialistaManutencao")]
        public async Task<IActionResult> EspecialistaManutencao([FromBody] PerguntaRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Pergunta))
                return BadRequest("A pergunta não pode ser vazia.");

            PromptDto promptDto = new PromptEngineering().PromptManutencao(request.Pergunta);
            
            String texto = promptDto.FormataToString();
            GenerateContentResponse response = await _model.GenerateContentAsync(texto);

            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(response.Text, "application/json");

        }




        [HttpPost("EspecialistaOrdemServico")]
        public async Task<IActionResult> EspecialistaOrdemServico([FromBody] ManutentorDto manutentorDto)
        {
            if (manutentorDto == null || string.IsNullOrWhiteSpace(manutentorDto.Nome))
                return BadRequest(new { erro = "O nome do manutentor não pode ser vazio." });



            var lista = new OrdemServicoFactory().GerarListaOrdensServico(manutentorDto.Nome, "Manutentor 2");
            var prompt = new OrdemServicoFactory().ConverterParaTexto(lista);

            PromptDto promptDto = new PromptEngineering().PromptOrdemServico(prompt, "Amauri");

            String texto = promptDto.FormataToString();
            GenerateContentResponse response = await _model.GenerateContentAsync(texto);

            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(response.Text, "application/json");

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
            GenerateContentResponse response = await _model.GenerateContentAsync(texto);

            //var json Ok(new
            //{
            //    Pergunta = request.Pergunta,
            //    Resposta = response.Text
            //});

            return Content(response.Text, "application/json");

        }


    }

 
}
