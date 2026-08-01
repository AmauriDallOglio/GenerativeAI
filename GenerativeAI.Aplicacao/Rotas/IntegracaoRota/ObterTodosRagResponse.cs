namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ObterTodosRagResponse
    {
        public string Conteudo { get; set; } = string.Empty;
        public int Page { get; set; }
        public int PageSize { get; set; }

        public static ObterTodosRagResponse Criar(string conteudo, int page, int pageSize)
        {
            return new ObterTodosRagResponse
            {
                Conteudo = conteudo,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
