using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services.Processo
{
    public interface IProcessoService
    {
        Task<ProcessoDTO.RespostaDTO?> GetProcessoAsync(string numeroProcesso);

    }
}
