using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Plantao;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // issue #39, cards Coilab #20260145/#20260146 — versão REAL (via Verde),
    // diferente do fake local que hoje existe só em maria-ia-back-end
    // (tabela Prisma própria, sem ligação com o Verde).
    [Route("api/plantao")]
    public class PlantaoController : ApiControllerBase
    {
        private readonly IPlantaoService _client;

        public PlantaoController(IPlantaoService plantaoService)
        {
            _client = plantaoService;
        }

        ///<summary>
        ///Consulta o plantão vigente no Verde.
        ///</summary>
        [HttpGet("vigente")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Vigente()
        {
            try
            {
                var plantao = await _client.GetVigenteAsync();
                return Ok(plantao);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar plantão vigente.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
