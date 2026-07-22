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

        private async Task<string> GetPessoaAsync(string cpfPessoa)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"pessoa?cpf={cpfPessoa}");

            request.Headers.Add("Authorization", _consultaVerdeSettings.Token);
            request.Headers.Add("X-Client-ID", _consultaVerdeSettings.ClientID);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((int)response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<JsonDocument> GetAssistidoAsync(string cpfPessoa)
        {
            var json = await GetPessoaAsync(cpfPessoa);

            return JsonDocument.Parse(json);
        }

        public async Task<AssistidoDTO.RespostaDTO> GetIdAssistidoAsync(string cpfPessoa)
        {
            var json = await GetPessoaAsync(cpfPessoa);

            return JsonSerializer.Deserialize<AssistidoDTO.RespostaDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
