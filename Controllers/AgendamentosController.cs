using GatewayConsultaApiVerde.Services;
using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Agendamentos;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [Route("api/agendamentos")]
    public class AgendamentosController : ApiControllerBase
    {
        private readonly IAgendamentosService _client;

        public AgendamentosController(IAgendamentosService agendamentosService)
        {
            _client = agendamentosService;
        }
        ///<summary>
        ///Realiza uma consulta à API do Verde utilizando o número do cpf para retornar os agendamentos do assistido.
        ///</summary>
        ///<remarks>
        ///Parâmetro: cpf. 
        ///O endpoint aceita o número do cpf em um dos seguintes formatos:
        ///00000000000
        ///000.000.000-00
        /// </remarks>
        ///<param name="cpf">Número do cpf do assistido</param>
        ///<returns>Dados de agendamentos do assistido</returns>
        [HttpGet("{cpf}")]
        [ProducesResponseType(typeof(AgendamentosResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return ErroParametroInvalido("Cpf do assistido obrigatório");
            }

            try
            {
                var processo = await _client.GetAgendamentosAsync(cpf);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar os agendamentos.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
