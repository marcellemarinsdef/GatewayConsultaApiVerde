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
        // "primeiro atendimento" (issue #39, card #20260146) — diferente de
        // Reagendar/Desmarcar, não parte de um idEvento existente.
        Task<(int StatusCode, JsonDocument? Body)> AgendarAsync(AgendarRequestDTO dados);
        Task<JsonDocument> VerificarDuplicadosAsync(int idPessoa, int idAssunto);
    }
}
