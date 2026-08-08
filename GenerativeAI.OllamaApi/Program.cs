
using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.OllamaApi.Middleware;
using GenerativeAI.OllamaApi.Util;
using GenerativeAI.Servico.Dto;
using GenerativeAI.Servico.Servicos;

namespace GenerativeAI.OllamaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
 
            builder.Services.Configure<OllamaAppSettingsDto>(builder.Configuration.GetSection("Ollama"));
 
            builder.Services.AddHttpClient<OllamaIntegracaoServico>();
            builder.Services.AddScoped<IOllamaIntegracaoServico>(sp => sp.GetRequiredService<OllamaIntegracaoServico>());
            builder.Services.AddScoped<OllamaAplicacaoServico>();

            builder.Services.AddControllers();

            //Swagger configurado com t�tulo e descri��o
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Ollama API",
                    Version = "v1",
                    Description = "API de integra��o com o Ollama (Llama3.2)"
                });
            });
 
            builder.Services.AddHttpClient();
 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();




            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddlewaresApi();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
           

            app.MapControllers();
 

            app.Run();
        }
    }
}
