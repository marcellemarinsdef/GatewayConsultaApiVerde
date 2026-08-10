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

        public async Task<CasosPaginacaoDTO> GetCasosAsync(
            string cpfPessoa,
            int offset = 0,
            int limit = 5)
        {
            var responseAssistido = await _assistidoService.GetIdAssistidoAsync(cpfPessoa);

            var idPessoa = responseAssistido?.Dados.Id
                ?? throw new Exception("Assistido não encontrado.");

            var json = await _consultaVerdeClient.GetAsync(
                $"caso/consultar-casos-pessoa/{idPessoa}");

            var dto = JsonSerializer.Deserialize<CasosPaginacaoDTO>(json);

            if (dto?.Dados == null)
            {
                return new CasosPaginacaoDTO
                {
                    Dados = [],
                    TotalCasos = 0,
                    Offset = offset,
                    Limit = limit,
                    TemMais = false,
                    ProximoOffset = offset
                };
            }

            var casos = dto.Dados
                .Where(c => c.Status.Equals("aberto", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var pagina = casos
                .Skip(offset)
                .Take(limit)
                .ToList();

            return new CasosPaginacaoDTO
            {
                Dados = pagina,
                TotalCasos = casos.Count,
                Offset = offset,
                Limit = limit,
                TemMais = offset + limit < casos.Count,
                ProximoOffset = offset + pagina.Count
            };
        }
    }

}
