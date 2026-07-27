using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Encaminhamento;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // issue #39, card Coilab #20260147 — "primeiro atendimento" sem hora
    // marcada (urgência/remoto/presencial). Alternativa a AgendamentoController
    // quando o órgão (ver OrgaoController) não exige vaga fixa.
    [Route("api/encaminhamento")]
    public class EncaminhamentoController : ApiControllerBase
    {
        private readonly IEncaminhamentoService _client;

        public EncaminhamentoController(IEncaminhamentoService encaminhamentoService)
        {
            _client = encaminhamentoService;
        }

        ///<summary>
        ///Cria um encaminhamento (primeiro atendimento sem hora marcada).
        ///</summary>
        [HttpPost("encaminhar")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Encaminhar([FromBody] EncaminharRequestDTO dados)
        {
            try
            {
                var (statusCode, body) = await _client.EncaminharAsync(dados);
                return StatusCode(statusCode, body);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao encaminhar.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
