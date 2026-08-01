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

            builder.Services.AddScoped<IRagIntegracaoServico>(sp => sp.GetRequiredService<RagIntegracao>());
            builder.Services.AddScoped<IMLNetIntegracaoServico>(sp => sp.GetRequiredService<MlNetIntegracao>());
            builder.Services.AddScoped<IntegracaoAplicacaoServico>();

            builder.Services.AddScoped<IContratoBaseHandler<ObterTodosRagRequest, ResultadoOperacao<object>>, ObterTodosRagHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<ImportarDocumentoRagRequest, ResultadoOperacao<object>>, ImportarDocumentoRagHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<GerarTreinamentoRequest, ResultadoOperacao<object>>, GerarTreinamentoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<ObterTreinamentoRequest, ResultadoOperacao<object>>, ObterTreinamentoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<ObterRespostaTreinamentoRequest, ResultadoOperacao<object>>,ObterRespostaTreinamentoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<AtualizarTreinamentoRequest, ResultadoOperacao<object>>, AtualizarTreinamentoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<ObterSessoesRequest, ResultadoOperacao<object>>, ObterSessoesHandler>();

            //Ollama
            builder.Services.AddScoped<IContratoBaseHandler<ObterTodosSessaoRequest, ResultadoOperacao>, ObterTodosSessaoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<ObterTodosDocumentoRequest, ResultadoOperacao>, ObterTodosDocumentoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<ImportarDocumentoRequest, ResultadoOperacao>, ImportarDocumentoHandler>();
            builder.Services.AddScoped<IContratoBaseHandler<PromptRequest, ResultadoOperacao>, PromptHandler>();

            builder.Services.AddScoped<PromptGenerativoDadosMocadosHandler>();
            builder.Services.AddScoped<PromptHandler>();
            builder.Services.AddScoped<PromptGenerativoHandler>();

            builder.Services.AddScoped<ITarefaDocumentoServico, TarefaDocumentoServico>();
            builder.Services.AddScoped<ITarefaMachineLearningServico, TarefaMachineLearningServico>();
            builder.Services.AddScoped<ITarefaSessaoMemoriaServico, TarefaSessaoMemoriaServico>();

            builder.Services.AddSingleton(typeof(IPrintaConsole<>), typeof(PrintaConsole<>));
            builder.Services.AddSingleton<ISessaoMemoriaServico, SessaoMemoriaServico>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddHttpClient<OllamaServico>();
            builder.Services.AddHttpClient<GemmaServico>();
            builder.Services.AddScoped<IPromptHelper, PromptServico>();

            builder.Services.AddScoped<IModeloMLRepositorio, ModeloMLRepositorio>();
            builder.Services.AddScoped<ISessaoCommandRepositorio, SessaoCommandRepositorio>();
            builder.Services.AddScoped<IDocumentoCommandRepositorio, DocumentoCommandRepositorio>();
            builder.Services.AddScoped<IDocumentoTrechoCommandRepositorio, DocumentoTrechoCommandRepositorio>();
            builder.Services.AddScoped<IDocumentoTrechoPalavraCommandRepositorio, DocumentoTrechoPalavraCommandRepositorio>();

            builder.Services.AddScoped<IMachineLearningServico, MachineLearningServico>();
            builder.Services.AddScoped<IOllamaServico, OllamaServico>();
            builder.Services.AddScoped<IGemmaServico, GemmaServico>();
            builder.Services.AddScoped<IRagServico, RagServico>();
            builder.Services.AddScoped<IGenerativoPipelineServico, GenerativoPipelineServico>();

            builder.Services.AddSingleton<MlCacheDto>();
            builder.Services.AddSingleton<RagCacheDto>();


        }
    }
}
