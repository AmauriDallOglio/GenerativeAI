using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GenerativeAI.Aplicacao.Servicos
{
    public class GenerativoPipelineServico : IGenerativoPipelineServico
    {

        private readonly IOllamaServico _ollamaServico;
        private readonly IMLNetIntegracaoServico _machineLearningServico;
        private readonly IRagIntegracaoServico _ragServico;
        private readonly IPrintaConsole<GenerativoPipelineServico> _printaConsole;
        private readonly RagCacheDto _cacheSistemaDto;

        public GenerativoPipelineServico(

            IOllamaServico ollamaServico,
            IMLNetIntegracaoServico machineLearningServico,
            IRagIntegracaoServico ragServico,
            IPrintaConsole<GenerativoPipelineServico> printaConsole)
        {
   
            _ollamaServico = ollamaServico;
            _machineLearningServico = machineLearningServico;
            _printaConsole = printaConsole;
            _ragServico = ragServico;

            if (_cacheSistemaDto == null)
            {
                var resultadoRag = _ragServico.ObterTodosAsync(1, 1000, new CancellationToken());
                _cacheSistemaDto = resultadoRag;
            }



        }

        public async Task<string> PerguntarAsync(string pergunta, CancellationToken cancellationToken)
        {



            Stopwatch stopwatch = Stopwatch.StartNew();
            _printaConsole.ImprimirSemCor("----------------------------------------------------------------------------");
            _printaConsole.ImprimirSemCor(pergunta);

            string resposta = string.Empty;


            float limiteConfiancaLm = Limite(NivelConfianca.NivelConfianca20);
            ResultadoOperacao<object> respostaMl = await _machineLearningServico.ObterRespostaTreinamentoAsync(pergunta, cancellationToken);
            string respostaResultado = respostaMl.Resultado?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(respostaResultado))
            {
                bool contemRespostaInvalida = _ollamaServico.ObterRespostasInvalidas()
                    .Any(frase => respostaResultado.Contains(frase, StringComparison.OrdinalIgnoreCase));

                if (!contemRespostaInvalida)
                {
                    stopwatch.Stop();
                    _printaConsole.ImprimirSemCor($"--> ML: {respostaResultado} - {stopwatch.ElapsedMilliseconds} ms");
                    return respostaResultado;
                }

                _printaConsole.ImprimirSemCor("--> ML: Sem resposta");
            }




            float limiteConfiancaRag = Limite(NivelConfianca.NivelConfianca40);
            List<string> trechosLocalizados = await FiltroPalavraChave(pergunta, limiteConfiancaRag, cancellationToken);
            if (trechosLocalizados.Count == 0)
            {
                resposta = "Desculpe, nao encontrei informacoes sobre isso na minha base de dados.";
                stopwatch.Stop();
                _printaConsole.ImprimirSemCor($"--> RAG: Sem resposta - {stopwatch.ElapsedMilliseconds} ms");
                return resposta;
            }

            _printaConsole.ImprimirSemCor($"--> RAG: Encontrado {trechosLocalizados.Count} trechos.");
            string promptGenerativo = await GerarPromptGenerativo(pergunta, trechosLocalizados, cancellationToken);

            resposta = await _ollamaServico.ExecutarPromptGeneraticoAsync(pergunta, promptGenerativo, "Sistema", cancellationToken);
 

            stopwatch.Stop();
            _printaConsole.ImprimirSemCor($"--> LLM: {resposta} - {stopwatch.ElapsedMilliseconds} ms");
            return resposta;
        }

        private bool RespostaValida(string resposta)
        {
            if (string.IsNullOrWhiteSpace(resposta))
                return false;

            return !_ollamaServico.ObterRespostasInvalidas()
                .Any(frase => resposta.Contains(frase, StringComparison.OrdinalIgnoreCase));
        }

         

 


        private enum NivelConfianca
        {
            NivelConfianca0,
            NivelConfianca05,
            NivelConfianca10,
            NivelConfianca20,
            NivelConfianca40,
            NivelConfianca60,
            NivelConfianca80,
            NivelConfianca100
        }

        private static float Limite(NivelConfianca nivel)
        {
            if (nivel == NivelConfianca.NivelConfianca0)
                return 0.0f;
            else if (nivel == NivelConfianca.NivelConfianca05)
                return 0.05f;
            else if (nivel == NivelConfianca.NivelConfianca10)
                return 0.1f;
            else if (nivel == NivelConfianca.NivelConfianca20)
                return 0.2f;
            else if (nivel == NivelConfianca.NivelConfianca40)
                return 0.4f;
            else if (nivel == NivelConfianca.NivelConfianca60)
                return 0.6f;
            else if (nivel == NivelConfianca.NivelConfianca80)
                return 0.8f;
            else if (nivel == NivelConfianca.NivelConfianca100)
                return 1.0f;
            else
                return 0.0f;
        }


        public async Task<string> GerarPromptGenerativo(string pergunta, List<string> trechosLocalizados, CancellationToken cancellationToken)
        {
            // Monta o prompt para o modelo de linguagem generativo, incluindo as regras obrigatórias e os trechos relevantes encontrados
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Você é um assistente que só pode responder usando o CONTEXTO fornecido.");
            sb.AppendLine($"Pergunta: " + pergunta);
            sb.AppendLine("--- CONTEXTO ---");
            foreach (string trecho in trechosLocalizados)
            {
                sb.AppendLine(trecho);
                sb.AppendLine();
            }
            sb.AppendLine("--- FIM DO CONTEXTO ---");
            sb.AppendLine("REGRAS:");
            sb.AppendLine("- Leia apenas o CONTEXTO acima.");
            sb.AppendLine("- Se a resposta estiver no CONTEXTO, responda usando apenas esse conteúdo.");
            sb.AppendLine("- Se a resposta NÃO estiver no CONTEXTO, responda EXATAMENTE:");
            sb.AppendLine("Desculpe, não encontrei informações sobre isso na minha base de dados.");
            sb.AppendLine("- Nunca use conhecimento externo.");
            return sb.ToString();
        }






        public async Task<List<string>> FiltroPalavraChave(string pergunta, float limiteConfianca, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<string> trechosCapturados = new();

            if (string.IsNullOrWhiteSpace(pergunta))
                return trechosCapturados;

            List<string> listaTokensPergunta = Tokenizar(pergunta);
            if (listaTokensPergunta.Count == 0)
                return trechosCapturados;

            int qtdTokensEncontrados = 0;



            foreach (string token in listaTokensPergunta)
            {
                if (_cacheSistemaDto.DocumentosTrechos.ContainsKey(token))
                {
                    qtdTokensEncontrados++;

                    List<string> frasesAssociadas = _cacheSistemaDto.DocumentosTrechos[token].Frases;
                    foreach (string frase in frasesAssociadas)
                    {
                        if (!trechosCapturados.Contains(frase))
                            trechosCapturados.Add(frase);
                    }
                }
            }

            double percentual = (double)qtdTokensEncontrados / listaTokensPergunta.Count;
            if (percentual < limiteConfianca)
            {
                trechosCapturados = new List<string>();
            }

            return trechosCapturados;
        }

        private static List<string> Tokenizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new List<string>();

            string textoNormalizado = texto.ToLowerInvariant();
            var tokens = Regex.Split(textoNormalizado, @"\W+")
                .Where(w => w.Length > 1 && !ignorarPalavras.Contains(w))
                .Distinct()
                .ToList();

            return tokens;
        }

        private static readonly HashSet<string> ignorarPalavras = new()
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","os","as","um","uma","uns","umas",
            "de","do","da","dos","das","em","no","na","nos","nas","ele","ela","mais","menos","mas","ou","se","que","porque","como","quando","onde",
            "foi","será","está","estão","era","eram","sou","somos","são",
            "tinha","tinham","houve","houveram","estava","estavam","seria","seriam",
            "para","por","com","sem","ou","mas","se","até","ser","será",
            "sua","seu","suas","seus","ao","aos","sobre","entre",
            "ate","apos","ja","nao","sim","pelo","pela","pelos","pelas","este","esta","estes","estas","pelos","pelas",
            "então","assim","também","ainda","pois","logo","portanto","contudo","todavia",
            "sempre","nunca","agora","depois","antes","aqui","ali","lá",
            "alguém","ninguém","todos","tudo","nada","cada","qualquer",
            "exemplo","tipo","coisa","caso","vez","forma","modo"
        };


        public class RagCacheDto
        {
            public List<Documento> Documentos { get; set; } = new();
            public Dictionary<string, DocumentosTrechosDto> DocumentosTrechos { get; set; } = new();
        }

        public class DocumentosTrechosDto
        {
            public string Palavra { get; set; } = string.Empty;
            public int Quantidade { get; set; }
            public List<string> Frases { get; set; } = new List<string>();
        }

        public class Documento
        {
            public int Id { get; set; }
            public string Titulo { get; set; } = string.Empty;
            public string Texto { get; set; } = string.Empty;

            public string? TipoArquivo { get; set; }   // PDF, TXT, DOCX
            public long? TamanhoArquivo { get; set; }  // em bytes

            public DateTime DataImportacao { get; set; } = DateTime.Now;
            public DateTime? DataAtualizacao { get; set; }

            public ICollection<DocumentoTrecho> Trechos { get; set; } = new List<DocumentoTrecho>();

            protected Documento() { }
        }

        public class DocumentoTrecho
        {
            public int Id { get; set; }
            public string Frase { get; set; } = string.Empty;

            public int? IdDocumento { get; set; }
            public Documento? Documento { get; set; }


            // Relacionamento
            public ICollection<DocumentoTrechoPalavra> Palavras { get; set; } = new List<DocumentoTrechoPalavra>();
        }
        public class DocumentoTrechoPalavra
        {
            public int Id { get; set; }
            public string Palavra { get; set; } = string.Empty;
            public int Quantidade { get; set; }

            public int IdDocumentoTrecho { get; set; }
            public DocumentoTrecho DocumentoTrecho { get; set; } = null!;
        }

    }
}
