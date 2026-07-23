using GatewayConsultaApiVerde.Models;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Agendamento
{
    public interface IAgendamentoService
    {
        Task<JsonDocument> GetDetalheAsync(int idEvento);
        Task<JsonDocument> GetVagasAsync(int idEvento);
        Task<(int StatusCode, JsonDocument? Body)> ReagendarAsync(ReagendarRequestDTO dados);
        Task<(int StatusCode, JsonDocument? Body)> DesmarcarAsync(DesmarcarRequestDTO dados);
    }
}
