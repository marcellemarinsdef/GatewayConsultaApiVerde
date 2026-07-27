using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Bloqueio
{
    public interface IBloqueioService
    {
        Task<JsonDocument> GetBloqueioAsync(int idPessoa, int idAssunto, int idOrgao);
    }
}
