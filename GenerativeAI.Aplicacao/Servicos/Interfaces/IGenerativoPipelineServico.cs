namespace GenerativeAI.Aplicacao.Servicos.Interfaces
{
    public interface IGenerativoPipelineServico
    {
        Task<string> PerguntarAsync(string pergunta, CancellationToken cancellationToken);
    }
}
