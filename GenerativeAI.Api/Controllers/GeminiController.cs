using GenerativeAI.Api.Perguntas;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : ControllerBase
    {
 

        private readonly GenerativeModel _model;

        private readonly PerguntarSobreCarros _PerguntarSobreCarros; 


        public GeminiController(GenerativeModel model, PerguntarSobreCarros perguntarSobreCarros)
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

            _model = model;
            _PerguntarSobreCarros = perguntarSobreCarros;
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

   

    
        [HttpPost("perguntarCarros2")]
        public async Task<IActionResult> PerguntarSobreCarros2([FromBody] PerguntaRequest perguntaRequest)
        {
            if (string.IsNullOrWhiteSpace(perguntaRequest.Pergunta))
            {
                return BadRequest(new { error = "O campo 'pergunta' não pode ser vazio." });
            }

            try
            {
                // --- ETAPA 1: BUSCAR CONTEXTO (Retrieval) Usamos nosso serviço para encontrar os trechos de texto mais relevantes para a pergunta do usuário em nossa base de conhecimento.
                Console.WriteLine($"Buscando contexto para a pergunta: '{perguntaRequest.Pergunta}'");
                List<string> dadosRelevantes = _PerguntarSobreCarros.BuscarInformacoes(perguntaRequest.Pergunta);

                // Juntamos os trechos encontrados em uma única string de contexto.
                string contexto = string.Join("\n---\n", dadosRelevantes);
                Console.WriteLine($"Contexto encontrado:\n{contexto}");


                // --- ETAPA 2: MONTAR O PROMPT AUMENTADO, criamos o prompt final, combinando as instruções, o contexto e a pergunta.
                string promptFinal = $@"
                    Você é um assistente virtual especialista em automóveis.
                    Sua tarefa é responder à pergunta do usuário baseando-se ÚNICA E EXCLUSIVAMENTE no contexto fornecido abaixo.
                    - Se a informação estiver no contexto, responda de forma clara e direta.
                    - Se a informação não estiver no contexto, responda exatamente com: 'Desculpe, não encontrei informações sobre isso na minha base de dados.'
                    - Não utilize nenhum conhecimento prévio ou externo.

                    --- CONTEXTO ---
                    {contexto}
                    --- FIM DO CONTEXTO ---

                    Pergunta do usuário: {perguntaRequest.Pergunta}

                    Resposta:
                    ";
                Console.WriteLine($"\n--- PROMPT FINAL ENVIADO PARA A GEMINI ---\n{promptFinal}");


                // --- ETAPA 3: CHAMAR A API GEMINI --- enviamos o prompt completo e detalhado para o modelo.
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

    public class PerguntaRequest
    {
        public string Pergunta { get; set; }
    }
}
