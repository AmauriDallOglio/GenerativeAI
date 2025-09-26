namespace GenerativeAI.Api.Perguntas
{
 

    public class PerguntarSobreCarros
    {
        private readonly List<string> _database;

        public PerguntarSobreCarros()
        {
            // Nossa base de dados falsa sobre carros
            _database = new List<string>
            {
                "Manual do Honda Civic 2022: A troca de óleo do motor 2.0L deve ser feita a cada 10.000 km ou 12 meses. O óleo recomendado é o 0W-20 sintético. A capacidade do cárter é de 4.2 litros.",
                "Especificações Técnicas - Toyota Corolla 2023: O motor 1.8L Hybrid tem uma potência combinada de 122 cv. O consumo na cidade é de 17.9 km/l.",
                "Guia de Manutenção - Fiat Cronos: A calibragem recomendada para os pneus aro 15 é de 32 PSI em todas as rodas para uso normal.",
                "Dicas do Fórum: Para resetar a luz de manutenção do Honda Civic após a troca de óleo, segure o botão 'trip' no painel com a ignição ligada até a luz piscar e apagar."
            };
        }

        // Método de busca simulado (bem simples, apenas para o exemplo)
        public List<string> BuscarInformacoes(string pergunta)
        {
            // Em um sistema real, aqui você converteria a 'pergunta' em um embedding
            // e faria uma busca por similaridade no seu Vector Database.

            // Para simular, vamos apenas encontrar os textos que contêm palavras-chave da pergunta.
            var palavrasChave = pergunta.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return _database
                .Where(texto => palavrasChave.Any(palavra => texto.ToLower().Contains(palavra)))
                .ToList();
        }
    }
}
