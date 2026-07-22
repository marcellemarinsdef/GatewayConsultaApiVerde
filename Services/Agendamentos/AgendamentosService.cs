using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Assistido;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Xml.Linq;

namespace GatewayConsultaApiVerde.Services.Agendamentos
{
    public class AgendamentosService : IAgendamentosService
    {
        private readonly HttpClient _httpClient;
        private readonly IAssistidoService _assistidoService;
        private readonly ConsultaVerdeSettings _consultaVerdeSettings;

        public AgendamentosService(HttpClient httpClient, IOptions<ConsultaVerdeSettings> consultaVerdeSettings, IAssistidoService assistidoService)
        {
            _httpClient = httpClient;
            _consultaVerdeSettings = consultaVerdeSettings.Value;
            _assistidoService = assistidoService;
        }

        public async Task<JsonDocument> GetAgendamentosAsync(string cpfPessoa)
        {
            var responseAssistido = await _assistidoService.GetIdAssistidoAsync(cpfPessoa);

            var idPessoa = responseAssistido?.Dados.Id;

            var request = new HttpRequestMessage(HttpMethod.Get, $"agendamento/listar-agendamentos-pessoa/{idPessoa}");
            request.Headers.Add("Authorization", _consultaVerdeSettings.Token);
            request.Headers.Add("X-Client-ID", _consultaVerdeSettings.ClientID);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((int)response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonDocument.Parse(json);
        }

    }
}
