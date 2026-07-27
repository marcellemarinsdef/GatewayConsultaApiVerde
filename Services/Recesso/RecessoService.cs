using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Recesso
{
    public class RecessoService : IRecessoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public RecessoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<JsonDocument> GetVigenteAsync() =>
            _consultaVerdeClient.GetAsync("recesso/vigente");
    }
}
