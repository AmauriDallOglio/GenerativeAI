using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Servico;
using GenerativeAI.Servico.Servicos;

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
            //    Console.WriteLine($"ERRO FATAL: Falha ao ler a chave da API do arquivo: {ex.Message}. A API n�o funcionar�.");
            //}

            builder.Services.Configure<AppSettingsDto>(builder.Configuration.GetSection("Apis"));
            builder.Services.AddSingleton<GenerativeModelServico>();
            builder.Services.AddSingleton(sp =>
            {
                var servico = sp.GetRequiredService<GenerativeModelServico>();
                var apiKey = servico.ObterChave();

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new InvalidOperationException("A chave da API Gemini não foi carregada. Defina Gemini:ApiKey no appsettings ou nas variáveis de ambiente.");
                }

                return new GenerativeModel(apiKey: apiKey, model: "gemini-2.5-flash");
            });

            builder.Services.AddHttpClient<RagServico>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Apis:Rag:Url"] ?? "https://localhost:7001");
            });

            builder.Services.AddHttpClient<MLNetServico>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Apis:MLNet:Url"] ?? "https://localhost:7002");
            });



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
