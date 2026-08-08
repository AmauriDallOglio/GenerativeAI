using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using GenerativeAI.Servico.Dto;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace GenerativeAI.Servico.Servicos
{
    public class OllamaIntegracaoServico : IOllamaIntegracaoServico
    {
        private readonly HttpClient _httpClient;
        public static readonly string _servidorLocalNome = "Local";
        public static readonly string _servidorLocalUrlBase = "http://localhost:11434";
        public static readonly string _servidorLocalModelo = "llama3.2";
        public static readonly int _servidorLocalTempoLimiteSegundos = 500;
        public static readonly string _servidorLocalIdioma = "pt-BR";
        private readonly IPrintaConsole<OllamaIntegracaoServico> _iPrintaConsole;
        private readonly OllamaAppSettingsDto _OllamaAppSettingsDto;

        public OllamaIntegracaoServico(HttpClient httpClient, IPrintaConsole<OllamaIntegracaoServico> iPrintaConsole, IOptions<OllamaAppSettingsDto> ollamaAppSettingsDto )
        {
            _httpClient = httpClient;
            _iPrintaConsole = iPrintaConsole;
            _OllamaAppSettingsDto = ollamaAppSettingsDto.Value;
        }

        public async Task<string> ExecutarPromptGeneraticoAsync(string pergunta, string promptMontado, string usuario, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew(); // inicia a contagem



            // Configura os parâmetros de temperatura e top_p para um estilo mais rigoroso, visando respostas mais objetivas e precisas.
            var (temperatura, topP) = ObterParametrosTemperatura(EstiloTemperatura.Rigoroso);

            // Monta o corpo da requisição para o servidor Ollama 
            var body = new
            {
                model = _servidorLocalModelo,
                prompt = promptMontado,
                stream = false,
                max_tokens = 512,
                options = new
                {
                    temperature = temperatura,
                    top_p = topP,
                    language = _servidorLocalIdioma,
                }
            };
            string resposta = string.Empty;

            //  Cria um CTS com timeout e linka ao token externo
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(_servidorLocalTempoLimiteSegundos));

            //Chama o método para enviar a requisição ao servidor Ollama e obter a resposta
            resposta = await ServidorOllama(_servidorLocalUrlBase, body, cts.Token);

            if (!string.IsNullOrEmpty(resposta))
            {
                // Verifica se a resposta contém alguma das frases inválidas
                bool contem = ObterRespostasInvalidas().Any(frase => resposta.Contains(frase, StringComparison.OrdinalIgnoreCase));
                if (contem)
                {
                    resposta = "Desculpe, não encontrei informações sobre isso na minha base de dados.";
                }
                //else

                //{
                //    await SalvarSessao(pergunta, promptMontado, resposta, cancellationToken);
                //}
            }

            stopwatch.Stop(); // para a contagem
            _iPrintaConsole.ImprimirSemCor($"--> Ollama Resposta: {stopwatch.ElapsedMilliseconds} ms");



            return resposta;
        }


        public async Task<ResultadoOperacao<string>> PerguntarAsync(string prompt, CancellationToken cancellationToken)
        {
            var body = new
            {
                model = _OllamaAppSettingsDto.Modelo,
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            try
            {
                _iPrintaConsole.Info($"Enviando pergunta para Ollama: {prompt}");
                var response = await _httpClient.PostAsync($"{_OllamaAppSettingsDto.BaseUrl}/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    _iPrintaConsole.Error($"Erro ao comunicar com Ollama: {erro}");
                    return ResultadoOperacao<string>.Falha($"Erro ao comunicar com o Ollama: {erro}");
                }

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var resposta = doc.RootElement.TryGetProperty("response", out var respostaJson)
                          ? respostaJson.GetString() ?? string.Empty
                          : json;
                return ResultadoOperacao<string>.Ok(resposta, "Resposta obtida com sucesso!");
            }
            catch (Exception ex)
            {
                _iPrintaConsole.Error("Erro ao logar a pergunta para Ollama");
                return ResultadoOperacao<string>.Falha($"Erro ao comunicar com o Ollama: {ex.Message}");
            }



        }

        public async Task<float[]> GerarEmbeddingAsync(string texto, CancellationToken cancellationToken)
        {
            try
            {
                var request = new
                {
                    model = "nomic-embed-text",
                    prompt = texto
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "http://localhost:11434/api/embeddings",
                    request,
                    cancellationToken);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                using var document = JsonDocument.Parse(json);
                var aa = document.RootElement.ToString();
                var embedding = document
                    .RootElement
                    .GetProperty("embedding")
                    .EnumerateArray()
                    .Select(x => x.GetSingle())
                    .ToArray();

                return embedding;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<string> ExecutarPromptAsync(string promptCompleto, CancellationToken cancellationToken)
        {
            var (temperatura, topP) = ObterParametrosTemperatura(EstiloTemperatura.Rigoroso);
            var body = new
            {
                model = _servidorLocalModelo,
                prompt = promptCompleto,
                stream = false,
                max_tokens = 512,
                options = new
                {
                    temperature = temperatura,
                    top_p = topP,
                    language = _servidorLocalIdioma,
                }
            };

            try
            {
                using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(_servidorLocalTempoLimiteSegundos));
                return await ServidorOllama(_servidorLocalUrlBase, body, cts.Token);
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // Timeout atingido
                // _logger.LogError(ex, "Timeout de {TempoLimite}s atingido para o servidor {TipoServidor}", tipoServidorConfig.TempoLimiteSegundos, tipoServidor);
                throw new TimeoutException($"Tempo limite de {_servidorLocalTempoLimiteSegundos}s atingido para {_servidorLocalModelo}");
            }
            catch (TaskCanceledException)
            {
                // Cancelamento externo
                // _logger.LogWarning(ex, "Operação cancelada externamente para o servidor {TipoServidor}", tipoServidor);
                throw;
            }
        }


        public List<string> ObterRespostasInvalidas()
        {
            var respostasInvalidas = new List<string>
            {
                "Desculpe, ",
                "Não tenho dados",
                "Infelizmente não consegui",
                "Não foi possível",
                "Não posso responder",
                "Não posso fornecer",
                "Olá! Estou aqui para ajudar"

            };

            return respostasInvalidas;
        }



        private async Task<string> ServidorOllama(string appSettingsUrlBase, object requestBody, CancellationToken cancellationToken, bool streaming = false)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            HttpResponseMessage? response = await _httpClient.PostAsync($"{appSettingsUrlBase}/api/generate", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync(cancellationToken);
                // _logger.LogError("Erro Ollama ({Status}): {Erro}", response.StatusCode, erro);
                throw new HttpRequestException(erro);
            }

            StringBuilder respostaFinal = new StringBuilder();
            using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using StreamReader reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                String? linha = await reader.ReadLineAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(linha)) continue;

                try
                {
                    using JsonDocument json = JsonDocument.Parse(linha);
                    if (json.RootElement.TryGetProperty("response", out var parte))
                        respostaFinal.Append(parte.GetString());
                }
                catch (JsonException)
                {
                    // ignora linha inválida
                }
            }
            return respostaFinal.ToString().Trim();
        }






        private enum EstiloTemperatura
        {
            Rigoroso = 0, //Útil para tarefas que exigem precisão, como explicações técnicas, cálculos ou respostas.
            Flexivel = 1, //Bom para quando você quer respostas variadas, mas ainda com certo controle, como brainstorming moderado ou textos explicativos.
            Criativo = 2 //Ideal para tarefas criativas, como histórias, metáforas, ideias fora da caixa ou geração de conteúdo artístico.
        }

        /// <summary>
        /// Temperatura baixa (0.1, 0.8) O modelo gera respostas mais determinísticas, objetivas e previsíveis.
        /// Temperatura intermediária (0.5, 0.9). Equilíbrio entre consistência e criatividade.
        /// Temperatura alta (0.9, 1.0). O modelo gera respostas mais diversas, imaginativas e menos previsíveis.
        /// Top_p baixo (0.2–0.4):  Ideal para respostas técnicas, precisas.  
        /// Top_p médio (0.6–0.8):  Bom equilíbrio entre consistência e diversidade. 
        /// Top_p alto (0.9–1.0):   Util para tarefas criativas, como histórias, brainstorming ou geração de ideias.
        /// </summary>
        /// <param name="estilo"></param>
        /// <returns></returns>
        private (double temperatura, double topP) ObterParametrosTemperatura(EstiloTemperatura estilo)
        {
            switch (estilo)
            {
                case EstiloTemperatura.Rigoroso:
                    return (0.1, 0.3);

                case EstiloTemperatura.Flexivel:
                    return (0.5, 0.7);

                case EstiloTemperatura.Criativo:
                    return (0.9, 1.0);

                default:
                    return (0.7, 0.9);
            }
        }



    }
}
