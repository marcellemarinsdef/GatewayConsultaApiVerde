using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Assistido;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [Route("api/assistido")]
    public class AssistidoController : ApiControllerBase
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
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
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
                var processo = await _client.GetAssistidoAsync(cpf);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar o cpf.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
