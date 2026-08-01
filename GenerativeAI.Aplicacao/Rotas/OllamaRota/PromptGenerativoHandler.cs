using GenerativeAI.Aplicacao.Util;
using System.Diagnostics;

namespace GenerativeAI.Aplicacao.Rotas.OllamaRota
{
    public class PromptGenerativoHandler : IContratoBaseHandler<PromptGenerativoRequest, ResultadoOperacao<object>>
    {
        private readonly IGenerativoPipelineServico _generativoPipeline;
        public PromptGenerativoHandler(IGenerativoPipelineServico generativoPipeline)
        {
            _generativoPipeline = generativoPipeline;
        }

        public async Task<ResultadoOperacao<object>> Executar(PromptGenerativoRequest request, CancellationToken cancellationToken = default)
        {
            Stopwatch tempo = Stopwatch.StartNew();

            if (string.IsNullOrWhiteSpace(request.Pergunta))
            {
                tempo.Stop();
                return ResultadoOperacao<object>.GerarErro("Campos devem ser informados!", 400);
            }

            string resultado = await _generativoPipeline.PerguntarAsync(request.Pergunta, cancellationToken);
            if (string.IsNullOrEmpty(resultado))
            {
                tempo.Stop();
                return ResultadoOperacao<object>.GerarErro("Desculpe, não encontrei informações sobre isso na minha base de dados.", 500);
            }

            tempo.Stop();
            if (!string.IsNullOrEmpty(resultado))
            {
                PromptGenerativoResponse response = PromptGenerativoResponse.Criar(request.Pergunta, resultado, tempo.ElapsedMilliseconds);
                return ResultadoOperacao<object>.GerarSucesso(response);
            }
            return ResultadoOperacao<object>.GerarErro("Não foi possivel gerar resposta.", 500);
        }
    }
}
