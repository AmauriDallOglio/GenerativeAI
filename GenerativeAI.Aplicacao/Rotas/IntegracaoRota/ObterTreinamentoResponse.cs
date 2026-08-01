namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterTreinamentoResponse
    {
        public string Conteudo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;

        public static ObterTreinamentoResponse Criar(string conteudo, string mensagem)
        {
            return new ObterTreinamentoResponse
            {
                Conteudo = conteudo,
                Mensagem = mensagem
            };
        }
    }
}
