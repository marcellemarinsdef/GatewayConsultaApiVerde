using System.Text.Json;
using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services.Casos
{
    public interface ICasosService
    {
        Task<CasosDTO.RespostaDto> GetCasosAsync(string cpfPessoa);
    }
}
