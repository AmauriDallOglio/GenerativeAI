using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using System.Diagnostics;

namespace GenerativeAI.Aplicacao.Rotas.OllamaRota
{
    public class PromptHandler : IContratoBaseHandler<PromptRequest, ResultadoOperacao<object>>
    {
        private readonly IOllamaIntegracaoServico _ollamaServico;

        public PromptHandler(IOllamaIntegracaoServico ollamaServico)
        {
            _ollamaServico = ollamaServico;
        }

        public async Task<ResultadoOperacao<object>> Executar(PromptRequest request, CancellationToken cancellationToken = default)
        {
            var tempo = Stopwatch.StartNew();

            if (string.IsNullOrWhiteSpace(request.Pergunta))
            {
                tempo.Stop();
                return ResultadoOperacao<object>.GerarErro("A pergunta deve ser informada.", 400);
            }
 

            var resposta = await _ollamaServico.ExecutarPromptAsync(request.Pergunta, cancellationToken);

            tempo.Stop();
            if (!string.IsNullOrEmpty(resposta))
            {
                var response = PromptResponse.Criar(request.Pergunta, resposta, tempo.ElapsedMilliseconds);
                return ResultadoOperacao<object>.GerarSucesso(response);
            }
            else
            {
                return ResultadoOperacao<object>.GerarErro($"Não foi possível gerar resposta. {tempo.ElapsedMilliseconds}", 500);
            }
        }
    }
}
