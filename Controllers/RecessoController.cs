using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Recesso;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // issue #39, cards Coilab #20260144/#20260146 — versão REAL (via Verde),
    // diferente do fake local que hoje existe só em maria-ia-back-end.
    [Route("api/recesso")]
    public class RecessoController : ApiControllerBase
    {
        private readonly IRecessoService _client;

        public RecessoController(IRecessoService recessoService)
        {
            _client = recessoService;
        }

        ///<summary>
        ///Consulta o recesso vigente no Verde.
        ///</summary>
        [HttpGet("vigente")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Vigente()
        {
            try
            {
                var recesso = await _client.GetVigenteAsync();
                return Ok(recesso);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar recesso vigente.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
