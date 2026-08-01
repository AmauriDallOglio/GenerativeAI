namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IOllamaServico
    {
        public Task<string> ExecutarPromptGeneraticoAsync(string pergunta, string promptMontado, string usuario, CancellationToken cancellationToken);
        public Task<string> ExecutarPromptAsync(string promptCompleto, CancellationToken cancellationToken);

        public Task<float[]> GerarEmbeddingAsync(string texto, CancellationToken cancellationToken);

        List<string> ObterRespostasInvalidas();
    }
}
