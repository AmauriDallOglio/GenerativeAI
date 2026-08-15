using GenerativeAI.Aplicacao.Rotas.IntegracaoRota;
using GenerativeAI.Aplicacao.Rotas.OllamaRota;
using GenerativeAI.Aplicacao.Servicos;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using GenerativeAI.Servico.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAI.Api.Configuracao
{
    public static class InjecaoDependenciaConfiguracao
    {
        public static void RegistrarServicos(WebApplicationBuilder builder)
        {


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            //Rotas 
            // Serviços principais
            builder.Services.AddScoped<IRagIntegracaoServico, RagIntegracao>();
            builder.Services.AddScoped<IMLNetIntegracaoServico, MlNetIntegracao>();
            builder.Services.AddScoped<IYoutubeIntegracaoServico, YoutubeIntegracao>();
            builder.Services.AddScoped<IWhisperIntegracaoServico, WhisperIntegracao>();
            builder.Services.AddScoped<IOllamaIntegracaoServico, OllamaIntegracaoServico>();
            builder.Services.AddScoped<IGenerativoPipelineServico, GenerativoPipelineServico>();

            // Handlers concretos (sem interface genérica)
            builder.Services.AddScoped<ObterTodosRagHandler>();
            builder.Services.AddScoped<ImportarDocumentoRagHandler>();
            builder.Services.AddScoped<GerarTreinamentoHandler>();
            builder.Services.AddScoped<ObterTreinamentoHandler>();
            builder.Services.AddScoped<ObterRespostaTreinamentoHandler>();
            builder.Services.AddScoped<AtualizarTreinamentoHandler>();
            builder.Services.AddScoped<ObterSessoesHandler>();
            builder.Services.AddScoped<BaixarAudioYoutubeHandler>();
            builder.Services.AddScoped<TranscricaoAudioWhisperHandler>();

            //builder.Services.AddScoped<IContratoBaseHandler<PromptRequest, ResultadoOperacao<object>>, PromptHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<ObterTodosRagRequest, ResultadoOperacao<object>>, ObterTodosRagHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<ImportarDocumentoRagRequest, ResultadoOperacao<object>>, ImportarDocumentoRagHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<GerarTreinamentoRequest, ResultadoOperacao<object>>, GerarTreinamentoHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<ObterTreinamentoRequest, ResultadoOperacao<object>>, ObterTreinamentoHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<ObterRespostaTreinamentoRequest, ResultadoOperacao<object>>, ObterRespostaTreinamentoHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<AtualizarTreinamentoRequest, ResultadoOperacao<object>>, AtualizarTreinamentoHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<ObterSessoesRequest, ResultadoOperacao<object>>, ObterSessoesHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<PromptRequest, ResultadoOperacao<object>>, PromptHandler>();
            //builder.Services.AddScoped<IContratoBaseHandler<PromptGenerativoRequest, ResultadoOperacao<object>>, PromptGenerativoHandler>();

            //Classe concreta (sem interface).
            builder.Services.AddScoped<PromptHandler>();
            builder.Services.AddScoped<PromptGenerativoHandler>();


            //Serviços de integração
            builder.Services.AddScoped<IRagIntegracaoServico>(sp => sp.GetRequiredService<RagIntegracao>());
            builder.Services.AddScoped<IMLNetIntegracaoServico>(sp => sp.GetRequiredService<MlNetIntegracao>());
            builder.Services.AddScoped<IYoutubeIntegracaoServico>(sp => sp.GetRequiredService<YoutubeIntegracao>());
            builder.Services.AddScoped<IWhisperIntegracaoServico>(sp => sp.GetRequiredService<WhisperIntegracao>());
            builder.Services.AddScoped<IGenerativoPipelineServico, GenerativoPipelineServico>();
            //builder.Services.AddHttpClient<OllamaIntegracaoServico>();
            builder.Services.AddScoped<IOllamaIntegracaoServico, OllamaIntegracaoServico>();

  




 

            builder.Services.AddSingleton(typeof(IPrintaConsole<>), typeof(PrintaConsole<>));
    
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
 
   
 


        }
    }
}
