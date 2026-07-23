using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Agendamentos;
using GatewayConsultaApiVerde.Services.Assistido;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Casos
{
    public class CasosService : ICasosService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;
        private readonly IAssistidoService _assistidoService;

        public CasosService(
            IConsultaVerdeClient consultaVerdeClient,
            IAssistidoService assistidoService)
        {
            _consultaVerdeClient = consultaVerdeClient;
            _assistidoService = assistidoService;
        }

        public async Task<CasosDTO.RespostaDto> GetCasosAsync(string cpfPessoa)
        {
            var responseAssistido = await _assistidoService.GetIdAssistidoAsync(cpfPessoa);


            var idPessoa = responseAssistido?.Dados.Id
                ?? throw new Exception("Assistido não encontrado.");

            var json = await _consultaVerdeClient.GetAsync(
                $"caso/consultar-casos-pessoa/{idPessoa}");

            var dto = JsonSerializer.Deserialize<CasosDTO.RespostaDto>(json);

            if (dto?.Dados != null)
            {
                dto.Dados = dto.Dados
                    .Where(c => c.Status.Equals("aberto", StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return dto;
        }
    }

}
