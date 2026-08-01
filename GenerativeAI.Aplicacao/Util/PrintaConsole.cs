using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace GenerativeAI.Aplicacao.Util
{
    public class PrintaConsole<T> : IPrintaConsole<T>
    {
        private readonly ILogger<T> _logger;
        public const string MeterName = "Ollama.Compartilhado.PrintaConsole";
        public static readonly Meter Meter = new(MeterName);

        public PrintaConsole(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Error(string mensagem)
        {
            Padrao(mensagem, ConsoleColor.White, ConsoleColor.Red, LogLevel.Error, "error");
        }

        public void Sucesso(string mensagem)
        {
            Padrao(mensagem, ConsoleColor.Black, ConsoleColor.Green, LogLevel.Information, "success");
        }

        public void Alerta(string mensagem)
        {
            Padrao(mensagem, ConsoleColor.Black, ConsoleColor.Yellow, LogLevel.Warning, "warning");
        }

        public void Info(string mensagem)
        {
            Padrao(mensagem, ConsoleColor.Yellow, ConsoleColor.Blue, LogLevel.Information, "info");
        }

        public void ImprimirSemCor(string mensagem)
        {

            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {mensagem}");

            //// Usa o nome completo do tipo para identificar a classe de origem no Grafana.
            //var classeOrigem = typeof(T).Name ?? typeof(T).FullName;
            // RegistrarLog(classeOrigem, "info");
        }

        private void Padrao(string mensagem, ConsoleColor fg, ConsoleColor bg, LogLevel level, string nivel)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {mensagem}");
            Console.ResetColor();

            _logger.Log(level, mensagem);

            // Usa o nome completo do tipo para identificar a classe de origem no Grafana.
            var classeOrigem = typeof(T).Name ?? typeof(T).FullName;
            RegistrarLog(classeOrigem, nivel);
        }



        // Contador de mensagens logadas por classe, nivel e fase.
        private static readonly Counter<long> LogCounter = Meter.CreateCounter<long>("ollama_log_total", unit: "logs", description: "Total de mensagens logadas");
        private static readonly Counter<long> ErrorCounter = Meter.CreateCounter<long>("ollama_log_error_total", unit: "logs", description: "Total de logs do tipo Error");
        private static readonly Counter<long> SucessoCounter = Meter.CreateCounter<long>("ollama_log_sucesso_total", unit: "logs", description: "Total de logs do tipo Sucesso");
        private static readonly Counter<long> AlertaCounter = Meter.CreateCounter<long>("ollama_log_alerta_total", unit: "logs", description: "Total de logs do tipo Alerta");
        private static readonly Counter<long> InfoCounter = Meter.CreateCounter<long>("ollama_log_info_total", unit: "logs", description: "Total de logs do tipo Info");

        // Tempo de execucao de operacoes em milissegundos.
        private static readonly Histogram<double> ExecutionTime = Meter.CreateHistogram<double>("ollama_execucao_ms", unit: "ms", description: "Tempo de execucao em milissegundos");

        public static void RegistrarLog(string classe, string nivel, string fase = "geral")
        {
            LogCounter.Add(1,
                new KeyValuePair<string, object?>("classe", classe),
                new KeyValuePair<string, object?>("nivel", nivel),
                new KeyValuePair<string, object?>("fase", fase));

            var labels = new[]
            {
                new KeyValuePair<string, object?>("classe", classe),
                new KeyValuePair<string, object?>("fase", fase)
            };

            switch (nivel)
            {
                case "error":
                    ErrorCounter.Add(1, labels);
                    break;
                case "success":
                    SucessoCounter.Add(1, labels);
                    break;
                case "warning":
                    AlertaCounter.Add(1, labels);
                    break;
                case "info":
                    InfoCounter.Add(1, labels);
                    break;
            }
        }

        public static void RegistrarExecucao(double milissegundos, string classe, string operacao)
        {
            ExecutionTime.Record(milissegundos,
                new KeyValuePair<string, object?>("classe", classe),
                new KeyValuePair<string, object?>("operacao", operacao));
        }
    }
}
