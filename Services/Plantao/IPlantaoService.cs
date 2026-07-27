using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Plantao
{
    public interface IPlantaoService
    {
        Task<JsonDocument> GetVigenteAsync();
    }
}
