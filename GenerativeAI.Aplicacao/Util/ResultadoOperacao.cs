namespace GenerativeAI.Aplicacao.Util
{
    public class ResultadoOperacao<T>
    {
        public bool Sucesso { get; init; }
        public string Mensagem { get; init; } = string.Empty;
        public object? Resultado { get; init; }
        public int? StatusCodigo { get; init; }

        public static ResultadoOperacao<T> Ok(T dados, string? mensagem = null)
        {
            return new ResultadoOperacao<T>
            {
                Sucesso = true,
                Mensagem = mensagem ?? "Operação realizada com sucesso.",
                Resultado = dados
            };
        }

        public static ResultadoOperacao<T> Falha(string mensagem, int? codigo = null, object? dados = null)
        {
            return new ResultadoOperacao<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                StatusCodigo = codigo,
                Resultado = dados
            };
        }

        public static ResultadoOperacao<T> GerarSucesso(object? dados = null, string? mensagem = null)
        {
            return new ResultadoOperacao<T>
            {
                Sucesso = true,
                Mensagem = mensagem ?? "Operação realizada com sucesso.",
                Resultado = dados
            };
        }

        public static ResultadoOperacao<T> GerarErro(string mensagem, int? codigo = null, object? dados = null)
        {
            return new ResultadoOperacao<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                StatusCodigo = codigo,
                Resultado = dados
            };
        }
    }
}
