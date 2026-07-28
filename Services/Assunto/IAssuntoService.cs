using GatewayConsultaApiVerde.Models;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Assunto
{
    public interface IAssuntoService
    {
        Task<JsonDocument> GetCategoriasAsync();
        Task<JsonDocument> ConsultarItemArvoreAsync(int? idCategoria, int? idItemCategoria);
        Task<(int StatusCode, JsonDocument? Body)> SugerirMotivoAsync(RelatoRequestDTO dados);
        Task<JsonDocument> GetAssuntoAsync(int idAssunto);
    }
}
