using GatewayConsultaApiVerde.Models.Responses;
using GatewayConsultaApiVerde.Services;
using GatewayConsultaApiVerde.Services.Agendamentos;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [ApiController]
    [Route("api/agendamentos")]
    public class AgendamentosController : Controller
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
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return BadRequest("Cpf obrigatório.");
            }

            try
            {
                var processo = await _client.GetAgendamentosAsync(cpf);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao consultar os agendamentos."
                );
            }
        }
    }
}
