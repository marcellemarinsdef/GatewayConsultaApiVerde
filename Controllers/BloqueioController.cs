using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Bloqueio;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // issue #39, card Coilab #20260148 (bloqueio de carência) — checa se a
    // pessoa está bloqueada de agendar/encaminhar de novo pro mesmo assunto/órgão.
    [Route("api/bloqueio")]
    public class BloqueioController : ApiControllerBase
    {
        private readonly IBloqueioService _client;

        public BloqueioController(IBloqueioService bloqueioService)
        {
            _client = bloqueioService;
        }

        ///<summary>
        ///Verifica se existe bloqueio de carência ativo pra pessoa/assunto/órgão.
        ///</summary>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get([FromQuery] int idPessoa, [FromQuery] int idAssunto, [FromQuery] int idOrgao)
        {
            if (idPessoa <= 0 || idAssunto <= 0 || idOrgao <= 0)
            {
                return ErroParametroInvalido("idPessoa, idAssunto e idOrgao obrigatórios");
            }

            try
            {
                var bloqueio = await _client.GetBloqueioAsync(idPessoa, idAssunto, idOrgao);
                return Ok(bloqueio);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar bloqueio.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
