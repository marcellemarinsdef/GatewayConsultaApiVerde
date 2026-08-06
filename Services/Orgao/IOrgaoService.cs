using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Orgao
{
    public interface IOrgaoService
    {
        Task<JsonDocument> GetPrimeiroAtendimentoAsync(int idPessoa, int idAssunto, string complemento);
    }
}
