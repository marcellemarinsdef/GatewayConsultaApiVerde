using ApiConsultaProcesso.Models;

namespace ApiConsultaProcesso.Services
{
    public interface IProcessoService
    {
        Task<ProcessoDTO.RespostaDTO?> GetProcessoAsync(string numeroProcesso);

    }
}
