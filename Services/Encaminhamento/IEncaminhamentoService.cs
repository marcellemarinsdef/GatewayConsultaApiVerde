using GatewayConsultaApiVerde.Models;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Encaminhamento
{
    public interface IEncaminhamentoService
    {
        Task<(int StatusCode, JsonDocument? Body)> EncaminharAsync(EncaminharRequestDTO dados);
    }
}
