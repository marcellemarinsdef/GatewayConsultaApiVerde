using GatewayConsultaApiVerde.Models;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Processo
{
    public interface IProcessoService
    {
        Task<JsonDocument> GetProcessoAsync(string numeroProcesso);

    }
}
