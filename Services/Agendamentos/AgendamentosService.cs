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

        public async Task<AgendamentosDTO.RespostaAgendamentosDTO> GetAgendamentosAsync(string cpfPessoa)
        {
            var clientId = _consultaVerdeSettings.ClientID;
            var token = _consultaVerdeSettings.Token;

            var requestAssistido = new HttpRequestMessage(HttpMethod.Get, $"pessoa?cpf={cpfPessoa}");
            requestAssistido.Headers.Add("Authorization", token);
            requestAssistido.Headers.Add("X-Client-ID", clientId);

            var responseAssistido = await _assistidoService.GetAssistidoAsync(cpfPessoa);

            var idPessoa = responseAssistido?.Dados?.Id;

            var request = new HttpRequestMessage(HttpMethod.Get, $"agendamento/listar-agendamentos-pessoa/{idPessoa}");
            request.Headers.Add("Authorization", token);
            request.Headers.Add("X-Client-ID", clientId);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((int)response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AgendamentosDTO.RespostaAgendamentosDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true

            });

        }

    }
}
