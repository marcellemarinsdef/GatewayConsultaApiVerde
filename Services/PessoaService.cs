using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services
{
    public class VerdeApiClient : IVerdeApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ConsultaVerdeSettings _consultaVerdeSettings;

        public VerdeApiClient(HttpClient httpClient, IOptions<ConsultaVerdeSettings> consultaVerdeSettings)
        {
            _httpClient = httpClient;
            _consultaVerdeSettings = consultaVerdeSettings.Value;
        }

        public async Task<PessoaDTO.RespostaDTO> GetPessoaAsync(string cpfPessoa)
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
            return JsonSerializer.Deserialize<PessoaDTO.RespostaDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,               
            });
        }
    }
}
