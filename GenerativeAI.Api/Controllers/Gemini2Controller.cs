using GenerativeAI.Servico;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Gemini2Controller : ControllerBase
    {
        private readonly GenerativeModel _model;
        public Gemini2Controller(IConfiguration configuration)
        {
            _model = new GenerativeModelServico(configuration).Obter();
        }


        [HttpPost("perguntar")]
        public async Task<IActionResult> Perguntar([FromBody] PerguntaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Pergunta))
            {
                return BadRequest(new { error = "O campo 'pergunta' não pode ser vazio." });
            }

            var response = await _model.GenerateContentAsync(request.Pergunta);
            return Ok(new { resposta = response.Text() });
        }


        [HttpPost("perguntarCarros")]
        public async Task<IActionResult> PerguntarSobreCarros([FromBody] PerguntaRequest perguntaRequest)
        {
            if (string.IsNullOrWhiteSpace(perguntaRequest.Pergunta))
            {
                return BadRequest(new { error = "O campo 'pergunta' não pode ser vazio." });
            }

            string promptFinal = $@"
                Você é um assistente especialista em carros. Sua única função é responder a perguntas sobre veículos usando ESTRITAMENTE as informações fornecidas no contexto abaixo.
                Se a resposta não estiver no contexto, diga apenas: 'Desculpe, não encontrei informações sobre isso na minha base de dados.'
                Não use nenhum conhecimento externo.

                Pergunta do usuário: {perguntaRequest.Pergunta}

                Resposta:
                ";

            var response = await _model.GenerateContentAsync(promptFinal);
            return Ok(new { resposta = response.Text() });
        }

    }

 
}
