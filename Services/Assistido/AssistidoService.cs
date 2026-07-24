using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using Microsoft.Extensions.Options;
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
    }
}
