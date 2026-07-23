using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.ConsultasBase
{
    public interface IConsultaVerdeClient
    {
        Task<JsonDocument> GetAsync(string endpoint);

        // StatusCode devolvido pra controller distinguir sucesso 200 de 204
        // (ex: desmarcar agendamento — ambos são sucesso, corpo diferente).
        Task<(int StatusCode, JsonDocument? Body)> PostAsync(string endpoint, object payload);
    }
}
