namespace GenerativeAI.Servico
{
    public class GenerativeModelServico
    {
 
 

        public GenerativeModelServico( )
        { 
 
        }

        public string ObterChave()
        {
            string apiKey = "";
            try
            {
                string filePath = "C:\\Amauri\\GitHub\\GeminiKey.txt";
                Console.WriteLine("Chave da API Gemini carregada com sucesso.");
                return apiKey = System.IO.File.ReadAllText(filePath).Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO FATAL: Falha ao ler a chave da API do arquivo: {ex.Message}. A API não funcionará.");
                return apiKey;
            }
        }
        public GenerativeModel Obter()
        {
            string apiKey;
            try
            {
                string filePath = "C:\\Amauri\\GitHub\\GeminiKey.txt";
                apiKey = System.IO.File.ReadAllText(filePath).Trim();
            }
            catch (Exception ex)
            {
                apiKey = "";
                Console.WriteLine($"Falha ao ler a chave da API do arquivo: {ex.Message}");
            }
            return new GenerativeModel(apiKey: apiKey, model: "gemini-2.5-flash");
        }
    }
}
