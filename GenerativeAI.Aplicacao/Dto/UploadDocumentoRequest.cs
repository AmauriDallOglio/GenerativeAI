using Microsoft.AspNetCore.Http;

namespace GenerativeAI.Aplicacao.Dto
{
    public class UploadDocumentoRequest
    {
        public IFormFile? Arquivo { get; set; }
    }
}
