using GenerativeAI.Aplicacao.Dto;
using GenerativeAI.Aplicacao.Servicos.Interfaces;
using GenerativeAI.Aplicacao.Util;
using Microsoft.Extensions.Options;

namespace GenerativeAI.Servico.Servicos
{
    public class MlNetIntegracao : IMLNetIntegracaoServico
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public MlNetIntegracao(HttpClient httpClient, IOptions<AppSettingsDto> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.MLNet.ApiKey;
        }

        public async Task<ResultadoOperacao<object>> GerarTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await ExecuteGetAsync("api/MLNet/GerarTreinamento", cancellationToken);
            return resultado;
        }
 

        public async Task<ResultadoOperacao<object>> ObterTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await ExecuteGetAsync("api/MLNet/ObterTreinamento", cancellationToken);
            return resultado;
        }
       
        public async Task<ResultadoOperacao<object>> ObterRespostaTreinamentoAsync(string pergunta, CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await ExecuteGetAsync("api/MLNet/ObterRespostaTreinamento", cancellationToken);
            return resultado;
        }
        

        public async Task<ResultadoOperacao<object>> AtualizarTreinamentoAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await ExecuteGetAsync("api/MLNet/AtualizarTreinamento", cancellationToken);
            return resultado;
        }
          

        public async Task<ResultadoOperacao<object>> ObterSessoesAsync(CancellationToken cancellationToken = default)
        {
            ResultadoOperacao<object> resultado = await ExecuteGetAsync("api/Sessoes/ObterTodos", cancellationToken);
            return resultado;
        }
            

        private async Task<ResultadoOperacao<object>> ExecuteGetAsync(string url, CancellationToken cancellationToken)
        {
            var response = await Executar(HttpMethod.Get, url, cancellationToken: cancellationToken);
            var conteudo = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return ResultadoOperacao<object>.GerarSucesso(conteudo, "Operação enviada ao ML.NET com sucesso.");
            }

            return ResultadoOperacao<object>.GerarErro($"Falha ao consultar o ML.NET. Status: {(int)response.StatusCode}. {conteudo}", (int)response.StatusCode);
        }

        private async Task<HttpResponseMessage> Executar(HttpMethod metodo, string url, HttpContent? content = null, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(metodo, url);
            request.Headers.Add("X-Api-Key", _apiKey);
            if (content is not null)
            {
                request.Content = content;
            }

            return await _httpClient.SendAsync(request, cancellationToken);
        }
    }
}
