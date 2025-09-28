using GenerativeAI.Servico;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Gemini2Controller : ControllerBase
    {
        private readonly GenerativeModel _model;
        public Gemini2Controller()
        {
            //string apiKey;
            //try
            //{
            //    string filePath = "C:\\Amauri\\GitHub\\GeminiKey.txt";
            //    apiKey = System.IO.File.ReadAllText(filePath).Trim();
            //}
            //catch (Exception ex)
            //{
            //    apiKey = "";
            //    Console.WriteLine($"Falha ao ler a chave da API do arquivo: {ex.Message}");
            //}
            //_model = new GenerativeModel(apiKey: apiKey, model: "gemini-2.5-flash");

            _model = new GenerativeModelServico().Obter();
        }


        [HttpPost("perguntar")]
        public async Task<IActionResult> Perguntar([FromBody] PerguntaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Pergunta))
            {
                return BadRequest(new { error = "O campo 'pergunta' não pode ser vazio." });
            }

            try
            {
                var response = await _model.GenerateContentAsync(request.Pergunta);
                return Ok(new { resposta = response.Text() });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na chamada da API Gemini: {ex.Message}");
                return StatusCode(500, new { error = "Ocorreu um erro ao se comunicar com a API Gemini.", details = ex.Message });
            }
        }


        [HttpPost("perguntarCarros")]
        public async Task<IActionResult> PerguntarSobreCarros([FromBody] PerguntaRequest perguntaRequest)
        {
            if (string.IsNullOrWhiteSpace(perguntaRequest.Pergunta))
            {
                return BadRequest(new { error = "O campo 'pergunta' não pode ser vazio." });
            }
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na chamada da API Gemini: {ex.Message}");
                return StatusCode(500, new { error = "Ocorreu um erro ao se comunicar com a API Gemini.", details = ex.Message });
            }
        }

    }

 
}
