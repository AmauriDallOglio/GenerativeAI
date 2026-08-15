using GenerativeAI.Aplicacao.Util;
using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Rotas.IntegracaoRota
{
    public class TranscricaoAudioWhisperRequest : IRequest<ResultadoOperacao<object>>
    {
        public IFormFile? Arquivo { get; set; }
    }
}
