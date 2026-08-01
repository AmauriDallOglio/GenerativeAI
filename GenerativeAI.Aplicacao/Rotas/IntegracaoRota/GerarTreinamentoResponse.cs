namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class GerarTreinamentoResponse
    {
        public string Conteudo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;

        public static GerarTreinamentoResponse Criar(string conteudo, string mensagem)
        {
            return new GerarTreinamentoResponse
            {
                Conteudo = conteudo,
                Mensagem = mensagem
            };
        }
    }
}
