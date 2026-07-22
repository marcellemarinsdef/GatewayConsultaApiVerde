using GatewayConsultaApiVerde.Services.Assistido;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [ApiController]
    [Route("api/assistido")]
    public class AssistidoController : Controller
    {
        private readonly IAssistidoService _client;

        public AssistidoController(IAssistidoService pessoaService)
        {
            _client = pessoaService;
        }
        ///<summary>
        ///Realiza uma consulta à API do Verde utilizando o número do cpf para retornar os dados do assistido.
        ///</summary>
        ///<remarks>
        ///<para>
        ///Parâmetro: cpf. 
        ///O endpoint aceita o número do cpf em um dos seguintes formatos:
        ///00000000000
        ///000.000.000-00
        ///</para>
        /// </remarks>
        ///<param name="cpf">Número do cpf do assistido</param>
        ///<returns>Dados de cadastro do assistido</returns>
        [HttpGet("{cpf}")]
        public async Task<IActionResult> Get(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return BadRequest("Cpf do assistido obrigatório");
            }

            try
            {
                var processo = await _client.GetAssistidoAsync(cpf);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao consultar o cpf."
                );
            }
        }
    }
}
