namespace GenerativeAI.Servico.Util
{
    public class Resultado<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Objeto { get; set; }

        public static Resultado<T> Ok(T resultado, string mensagem = "Operação realizada com sucesso!")
        {
            return new Resultado<T> { Sucesso = true, Mensagem = mensagem, Objeto = resultado };
        }

        public static Resultado<T> Falha(string mensagem)
        {
            return new Resultado<T> { Sucesso = false, Mensagem = mensagem };
        }
    }
}
