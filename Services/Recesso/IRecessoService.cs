using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Recesso
{
    public interface IRecessoService
    {
        Task<JsonDocument> GetVigenteAsync();
    }
}
