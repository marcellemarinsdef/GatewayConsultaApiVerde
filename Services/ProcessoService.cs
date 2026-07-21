using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace GatewayConsultaApiVerde.Services
{
    public class ProcessoService: IProcessoService
    {
        
        private readonly HttpClient _httpClient;
        private readonly ConsultaVerdeSettings _consultaVerdeSettings;
        public ProcessoService(HttpClient httpClient, IOptions<ConsultaVerdeSettings> consultaVerdeSettings)
        {
            _httpClient = httpClient;
            _consultaVerdeSettings = consultaVerdeSettings.Value;
        }

        public async Task<ProcessoDTO.RespostaDTO> GetProcessoAsync(string numeroProcesso)
        {
            var clientId = _consultaVerdeSettings.ClientID;
            var token = _consultaVerdeSettings.Token;

            var request = new HttpRequestMessage(HttpMethod.Get, $"processo/consultar/{numeroProcesso}" );
            request.Headers.Add("Authorization", token);
            request.Headers.Add("X-Client-ID", clientId);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((int)response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProcessoDTO.RespostaDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                 {
                      new DateTimeConverter()
                }
            });
        }
    }
}
