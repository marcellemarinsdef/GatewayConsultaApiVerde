using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Plantao
{
    public class PlantaoService : IPlantaoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public PlantaoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<JsonDocument> GetVigenteAsync() =>
            _consultaVerdeClient.GetAsync("plantao/vigente");
    }
}
