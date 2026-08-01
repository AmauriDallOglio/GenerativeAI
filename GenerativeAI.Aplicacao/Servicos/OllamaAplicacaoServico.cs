using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Servicos
{
    public class OllamaAplicacaoServico
    {
        private readonly IOllamaPerguntaServico _ollamaPerguntaServico;

        public OllamaAplicacaoServico(IOllamaPerguntaServico ollamaPerguntaServico)
        {
            _ollamaPerguntaServico = ollamaPerguntaServico;
        }

        public async Task<ResultadoOperacao<string>> PerguntarAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                return ResultadoOperacao<string>.Falha("Informe uma pergunta válida.");
            }

            return await _ollamaPerguntaServico.PerguntarAsync(prompt);
        }
    }
}
