
using GenerativeAI.Api.Perguntas;
using GenerativeAI.Servico;

namespace GenerativeAI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //string apiKey = "";
            //try
            //{
            //    string filePath = "C:\\Amauri\\GitHub\\GeminiKey.txt";
            //    apiKey = System.IO.File.ReadAllText(filePath).Trim();
            //    Console.WriteLine("Chave da API Gemini carregada com sucesso.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"ERRO FATAL: Falha ao ler a chave da API do arquivo: {ex.Message}. A API não funcionará.");
            //}

            string apiKey = new GenerativeModelServico().ObterChave();

            builder.Services.AddSingleton(sp =>
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("A chave da API Gemini não foi carregada. Verifique o caminho do arquivo e as permissões.");
                }
                return new GenerativeModel(apiKey: apiKey, model: "gemini-2.5-flash");
            });

            builder.Services.AddSingleton<PerguntarSobreCarros>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
