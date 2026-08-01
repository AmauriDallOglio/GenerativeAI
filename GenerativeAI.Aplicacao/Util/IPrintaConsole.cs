namespace GenerativeAI.Aplicacao.Util
{
    public interface IPrintaConsole<T>
    {
        void Error(string mensagem);
        void Sucesso(string mensagem);
        void Alerta(string mensagem);
        void Info(string mensagem);
        void ImprimirSemCor(string mensagem);
    }
}
