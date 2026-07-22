using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services.Agendamentos
{
    public interface IAgendamentosService
    {
        Task<AgendamentosDTO.RespostaAgendamentosDTO> GetAgendamentosAsync(string cpfPessoa);
    }
}
