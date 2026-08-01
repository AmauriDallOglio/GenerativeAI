using GenerativeAI.Api.Configuracao;

namespace GenerativeAI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string environmentName = builder.Environment.EnvironmentName;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            //string apiKey = "";
            //try
            //{
            //    string filePath = "C:\\Amauri\\GitHub\\GeminiKey.txt";
            //    apiKey = System.IO.File.ReadAllText(filePath).Trim();
            //    Console.WriteLine("Chave da API Gemini carregada com sucesso.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"ERRO FATAL: Falha ao ler a chave da API do arquivo: {ex.Message}. A API n�o funcionar�.");
            //}

            AppSettingsConfiguracao.Carregar(builder.Services, configuration);
            InjecaoDependenciaConfiguracao.RegistrarServicos(builder);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
        
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
