using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.ConsultasBase
{
    public interface IConsultaVerdeClient
    {
        Task<JsonDocument> GetAsync(string endpoint);
    }
}
