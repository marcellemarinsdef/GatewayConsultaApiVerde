using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services
{
    public interface IProcessoService
    {
        Task<ProcessoDTO.RespostaDTO?> GetProcessoAsync(string numeroProcesso);

    }
}
