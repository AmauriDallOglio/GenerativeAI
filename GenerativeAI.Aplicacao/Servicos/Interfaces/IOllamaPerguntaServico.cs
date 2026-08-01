using GenerativeAI.Aplicacao.Util;

namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IOllamaPerguntaServico
    {
        Task<ResultadoOperacao<string>> PerguntarAsync(string prompt);
    }
}
