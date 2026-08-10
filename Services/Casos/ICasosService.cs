using System.Text.Json;
using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services.Casos
{
    public interface ICasosService
    {
        Task<CasosPaginacaoDTO> GetCasosAsync(string cpfPessoa, int offset,
            int limit);
    }
}
