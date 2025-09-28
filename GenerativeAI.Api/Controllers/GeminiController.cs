using GenerativeAI.Api.Perguntas;
using GenerativeAI.Servico;
using GenerativeAI.Servico.Dto;
using GenerativeAI.Types;
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
            _model = model;
            _PerguntarSobreCarros = perguntarSobreCarros;
        }

        [HttpPost("perguntar")]
        public async Task<IActionResult> Perguntar([FromBody] PerguntaRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Pergunta))
                return BadRequest("A pergunta não pode ser vazia.");

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

        [HttpPost("PromptEspecialistaManutencao")]
        public async Task<IActionResult> PromptEspecialistaManutencao([FromBody] PerguntaRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Pergunta))
                return BadRequest("A pergunta não pode ser vazia.");


            string persona = @"Você é um especialista em manutenção de máquinas industriais e de construção civil e predial, 
                com amplo conhecimento em manutenção preventiva, preditiva e corretiva. Sua função é fornecer respostas técnicas, detalhadas e práticas, 
                considerando boas práticas e normas de segurança, sempre explique de forma clara e estruturada. 
                Se não souber a resposta, diga: Desculpe, não encontrei informações sobre isso na minha base de dados.";

            string contexto = @"
                - Tipos de máquinas: escavadeiras, guindastes, empilhadeiras, prensas industriais, tornos.
                - Manutenção preventiva: inspeções periódicas, lubrificação, troca de filtros, calibragem.
                - Manutenção preditiva: monitoramento por sensores (vibração, temperatura, pressão), análise de falhas, histórico de operação.
                - Manutenção corretiva: reparos após falha, substituição de peças danificadas, diagnóstico de problemas.
                - Normas de segurança: uso de EPIs, bloqueio de energia antes de manutenção, registro de manutenções.
                ";

            PromptDto promptDto = new PromptDto(persona, contexto, request.Pergunta);
            
            String texto = promptDto.ToString();
            GenerateContentResponse response = await _model.GenerateContentAsync(texto);

            return Ok(new
            {
                Pergunta = request.Pergunta,
                Resposta = response.Text
            });
        }

 
    }

 
}
