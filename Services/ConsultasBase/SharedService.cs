using GatewayConsultaApiVerde.Exceptions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.ConsultasBase
{
    public class ConsultaVerdeClient : IConsultaVerdeClient
    {
        private readonly HttpClient _httpClient;
        private readonly ConsultaVerdeSettings _settings;

        public ConsultaVerdeClient(
            HttpClient httpClient,
            IOptions<ConsultaVerdeSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<JsonDocument> GetAsync(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            request.Headers.Add("Authorization", _settings.Token);
            request.Headers.Add("X-Client-ID", _settings.ClientID);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new ApiException((int)response.StatusCode);

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
                throw new ApiException((int)response.StatusCode);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return ((int)response.StatusCode, null);

            var json = await response.Content.ReadAsStringAsync();
            return ((int)response.StatusCode, string.IsNullOrWhiteSpace(json) ? null : JsonDocument.Parse(json));
        }
    }

}
