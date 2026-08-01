namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterRespostaTreinamentoResponse
    {
        public string Conteudo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;

        public static ObterRespostaTreinamentoResponse Criar(string conteudo, string mensagem)
        {
            return new ObterRespostaTreinamentoResponse
            {
                Conteudo = conteudo,
                Mensagem = mensagem
            };
        }
    }
}
