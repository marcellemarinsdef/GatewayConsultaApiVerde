using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace GatewayConsultaApiVerde.Services.Processo
{
    public class ProcessoService: IProcessoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;
        private readonly ConsultaVerdeSettings _consultaVerdeSettings;

        public ProcessoService(IConsultaVerdeClient consultaVerdeClient, IOptions<ConsultaVerdeSettings> consultaVerdeSettings)
        {
            _consultaVerdeClient = consultaVerdeClient;
            _consultaVerdeSettings = consultaVerdeSettings.Value;
        }

        public async Task<JsonDocument> GetProcessoAsync(string numeroProcesso)
        { 
            return await _consultaVerdeClient.GetAsync(
                $"processo/consultar/{numeroProcesso}");
        }
    }
}
