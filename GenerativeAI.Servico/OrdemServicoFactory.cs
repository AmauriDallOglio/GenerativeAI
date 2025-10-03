namespace GenerativeAI.Servico
{
    using System.Text;

    public class OrdemServicoFactory
    {
        private static readonly Random _random = new Random();

        public List<OrdemServico> GerarListaOrdensServico(string manutentor1, string manutentor2)
        {
            var ordens = new List<OrdemServico>();
            DateTime hoje = DateTime.Now;
            int codigo = 1;

            // 10 passadas
            for (int i = 1; i <= 8; i++)
            {
                ordens.Add(CriarOrdem(codigo++, hoje.AddDays(-i), manutentor1, manutentor2));
            }

            // 5 no dia atual
            for (int i = 0; i < 12; i++)
            {
                ordens.Add(CriarOrdem(codigo++, hoje, manutentor1, manutentor2));
            }

            // 15 futuras
            for (int i = 1; i <= 15; i++)
            {
                ordens.Add(CriarOrdem(codigo++, hoje.AddDays(i), manutentor1, manutentor2));
            }

            return ordens;
        }


        //public List<OrdemServico> GerarListaOrdensServico(string manutentor, string manutentor2)
        //{
        //    var lista = new List<OrdemServico>();
        //    var hoje = DateTime.Today;
        //    int totalRegistros = 30;

        //    for (int i = 1; i <= totalRegistros; i++)
        //    {
        //        var os = new OrdemServico();
        //        os.Codigo = $"OS{i:0000}";

        //        if (i <= 10)
        //            os.DataCadastro = hoje.AddDays(-i); // passadas
        //        else if (i <= 15)
        //            os.DataCadastro = hoje; // atuais
        //        else
        //            os.DataCadastro = hoje.AddDays(i - 15); // futuras

        //        os.Status = GerarStatusAleatorio();
        //        os.TempoEstimadoHoras = new Random().Next(2, 9);
        //        os.Manutentor = (i % 2 == 0) ? "Manutentor 1" : "Manutentor 2";

        //        lista.Add(os);
        //    }

        //    return lista;
        //}


        private static OrdemServico CriarOrdem(int codigo, DateTime data, string manutentor1, string manutentor2)
        {
            return new OrdemServico
            {
                Codigo = codigo.ToString(),
                DataCadastro = data,
                Status = GerarStatusAleatorio(),
                TempoEstimadoHoras = (int)Math.Round(_random.NextDouble() * 8 + 1, 2), // entre 1 e 9 horas
                Manutentor = codigo % 2 == 0 ? manutentor1 : manutentor2
            };
        }

        private static StatusOrdemServico GerarStatusAleatorio()
        {
            var valores = Enum.GetValues(typeof(StatusOrdemServico));
            return (StatusOrdemServico)valores.GetValue(_random.Next(valores.Length))!;
        }

        public  string ConverterParaTexto(List<OrdemServico> ordens)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Lista de Ordens de Serviço:");
            foreach (var os in ordens)
            {
                sb.AppendLine($"- Código: {os.Codigo}, Data: {os.DataCadastro:dd/MM/yyyy}, " +
                              $"Status: {os.Status}, Tempo Estimado: {os.TempoEstimadoHoras}h, " +
                              $"Manutentor: {os.Manutentor}");
            }
            return sb.ToString();
        }

        public class OrdemServico
        {
            public string Codigo { get; set; } = string.Empty;
            public DateTime DataCadastro { get; set; }
            public StatusOrdemServico Status { get; set; }
            public int TempoEstimadoHoras { get; set; }
            public string Manutentor { get; set; } = string.Empty;
        }

        public enum StatusOrdemServico
        {
            Aberta,
            Agendada,
            EmExecucao,
            Parada,
            Finalizada
        }
    }

}
