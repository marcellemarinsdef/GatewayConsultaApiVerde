using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
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

        private HttpRequestMessage NovaRequisicaoPessoa(HttpMethod metodo, object corpo)
        {
            var request = new HttpRequestMessage(metodo, "pessoa")
            {
                Content = JsonContent.Create(corpo)
            };
            request.Headers.Add("Authorization", _consultaVerdeSettings.Token);
            request.Headers.Add("X-Client-ID", _consultaVerdeSettings.ClientID);
            return request;
        }

        public async Task<JsonDocument> CriarAssistidoAsync(CadastrarAssistidoRequestDTO dados)
        {
            var request = NovaRequisicaoPessoa(HttpMethod.Post, dados);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((int)response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json);
        }

        // Verde identifica a pessoa por idPessoa (numérico), não cpf — resolve
        // via GetIdAssistidoAsync antes de montar o payload (issue #31).
        public async Task<JsonDocument> AtualizarAssistidoAsync(string cpfPessoa, AtualizarAssistidoRequestDTO dados)
        {
            var pessoa = await GetIdAssistidoAsync(cpfPessoa);
            var idPessoa = pessoa?.Dados?.Id
                ?? throw new ApiException(404);

            var payload = new AtualizarPessoaVerdeDTO
            {
                IdPessoa = idPessoa,
                Endereco = dados.Endereco,
                Telefone = dados.Telefone,
                Email = dados.Email,
            };

            var request = NovaRequisicaoPessoa(HttpMethod.Put, payload);
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
