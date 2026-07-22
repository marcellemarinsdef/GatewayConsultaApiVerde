using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Assistido
{
    public class AssistidoService : IAssistidoService
    {
        private readonly HttpClient _httpClient;
        private readonly ConsultaVerdeSettings _consultaVerdeSettings;

        public AssistidoService(HttpClient httpClient, IOptions<ConsultaVerdeSettings> consultaVerdeSettings)
        {
            _httpClient = httpClient;
            _consultaVerdeSettings = consultaVerdeSettings.Value;
        }

        public async Task<AssistidoDTO.RespostaDTO> GetAssistidoAsync(string cpfPessoa)
        {
            var clientId = _consultaVerdeSettings.ClientID;
            var token = _consultaVerdeSettings.Token;

            var request = new HttpRequestMessage(HttpMethod.Get, $"pessoa?cpf={cpfPessoa}");
            request.Headers.Add("Authorization", token);
            request.Headers.Add("X-Client-ID", clientId);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((int)response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AssistidoDTO.RespostaDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
                
            });
        }
    }
}
