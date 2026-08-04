using GatewayConsultaApiVerde.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.ConsultasBase
{
    public class ConsultaVerdeClient : IConsultaVerdeClient
    {
        private readonly HttpClient _httpClient;
        private readonly ConsultaVerdeSettings _settings;
        private readonly ILogger<ConsultaVerdeClient> _logger;

        public ConsultaVerdeClient(
            HttpClient httpClient,
            IOptions<ConsultaVerdeSettings> settings,
            ILogger<ConsultaVerdeClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        // Lê o corpo do erro ANTES de jogar fora — sem isso, o cliente só via
        // "Parâmetro inválido" genérico pra qualquer 400 real do Verde, sem
        // pista nenhuma do motivo (achado investigando cadastro que sempre
        // falhava, maria-ia#20260202). Loga sempre (CloudWatch); repassa no
        // ApiException pra também aparecer na resposta de erro ao cliente.
        private async Task<ApiException> ErroComCorpo(HttpResponseMessage response, string metodo, string endpoint)
        {
            var corpo = await response.Content.ReadAsStringAsync();
            _logger.LogWarning(
                "Verde respondeu {Status} em {Metodo} {Endpoint}: {Corpo}",
                (int)response.StatusCode, metodo, endpoint, corpo);
            return new ApiException((int)response.StatusCode, string.IsNullOrWhiteSpace(corpo) ? null : corpo);
        }

        public async Task<JsonDocument> GetAsync(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            request.Headers.Add("Authorization", _settings.Token);
            request.Headers.Add("X-Client-ID", _settings.ClientID);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw await ErroComCorpo(response, "GET", endpoint);

            var json = await response.Content.ReadAsStringAsync();

            return JsonDocument.Parse(json);
        }

        public async Task<(int StatusCode, JsonDocument? Body)> PostAsync(string endpoint, object payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Add("Authorization", _settings.Token);
            request.Headers.Add("X-Client-ID", _settings.ClientID);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw await ErroComCorpo(response, "POST", endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return ((int)response.StatusCode, null);

            var json = await response.Content.ReadAsStringAsync();
            return ((int)response.StatusCode, string.IsNullOrWhiteSpace(json) ? null : JsonDocument.Parse(json));
        }

        public async Task<(int StatusCode, JsonDocument? Body)> PutAsync(string endpoint, object payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Add("Authorization", _settings.Token);
            request.Headers.Add("X-Client-ID", _settings.ClientID);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw await ErroComCorpo(response, "PUT", endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return ((int)response.StatusCode, null);

            var json = await response.Content.ReadAsStringAsync();
            return ((int)response.StatusCode, string.IsNullOrWhiteSpace(json) ? null : JsonDocument.Parse(json));
        }
    }

}
