using GatewayConsultaApiVerde.Models;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Agendamentos
{
    public interface IAgendamentosService
    {
        Task<JsonDocument> GetAgendamentosAsync(string cpfPessoa);
    }
}
