using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Assistido
{
    public class AssistidoService : IAssistidoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public AssistidoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
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

        public async Task<JsonDocument> CriarAssistidoAsync(CadastrarAssistidoRequestDTO dados)
        {
            var (_, body) = await _consultaVerdeClient.PostAsync("pessoa", dados);
            return body ?? JsonDocument.Parse("{}");
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
                DtNascimento = dados.DtNascimento,
            };

            var (_, body) = await _consultaVerdeClient.PutAsync("pessoa", payload);
            return body ?? JsonDocument.Parse("{}");
        }
    }
}
