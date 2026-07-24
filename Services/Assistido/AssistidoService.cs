using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Assistido
{
    public class AssistidoService : IAssistidoService
    {
        private readonly ConsultaVerdeSettings _consultaVerdeSettings;
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public AssistidoService(IConsultaVerdeClient consultaVerdeClient, IOptions<ConsultaVerdeSettings> consultaVerdeSettings)
        {
            _consultaVerdeClient = consultaVerdeClient;
            _consultaVerdeSettings = consultaVerdeSettings.Value;
        }

        public async Task<JsonDocument> GetAssistidoAsync(string cpfPessoa)
        {
            return await _consultaVerdeClient.GetAsync(
                $"pessoa?cpf={cpfPessoa}");
        }



        public async Task<AssistidoDTO.RespostaDTO> GetIdAssistidoAsync(string cpfPessoa)
        {
            var json = await GetAssistidoAsync(cpfPessoa);

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
