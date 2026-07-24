using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Processo;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [Route("api/processo")]
    public class ProcessoController : ApiControllerBase
    {
        private readonly IProcessoService _processoService;

        public ProcessoController(IProcessoService processoService)
        {
            _processoService = processoService;
        }
        ///<summary>
        ///Realiza uma consulta à API do Verde utilizando o número do processo informado pelo usuário.
        ///</summary>
        ///<remarks>
        ///<para>
        ///Parâmetro: numeroProcesso.
        ///O endpoint aceita o número do processo em um dos seguintes formatos:
        ///00000000000000000000
        ///0000000-00.0000.0.00.0000
        ///</para>
        /// </remarks>
        ///<param name="numeroProcesso">Número do processo no verde</param>
        ///<returns>Movimento do processo</returns>
        [HttpGet("{numeroProcesso}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(string numeroProcesso)
        {
            if (string.IsNullOrWhiteSpace(numeroProcesso))
            {
                return ErroParametroInvalido("Número do processo obrigatório");
            }

            try
            {
                var processo = await _processoService.GetProcessoAsync(numeroProcesso);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar o processo.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
