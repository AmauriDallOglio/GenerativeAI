namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class ImportarDocumentoRagResponse
    {
        public string Mensagem { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;

        public static ImportarDocumentoRagResponse Criar(string conteudo, string mensagem)
        {
            return new ImportarDocumentoRagResponse
            {
                Conteudo = conteudo,
                Mensagem = mensagem
            };
        }
    }
}
