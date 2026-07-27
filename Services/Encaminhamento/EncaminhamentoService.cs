using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Encaminhamento
{
    public class EncaminhamentoService : IEncaminhamentoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public EncaminhamentoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<(int StatusCode, JsonDocument? Body)> EncaminharAsync(EncaminharRequestDTO dados) =>
            _consultaVerdeClient.PostAsync("encaminhamento/encaminhar", dados);
    }
}
