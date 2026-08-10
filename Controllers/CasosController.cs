using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Bedrock;
using GatewayConsultaApiVerde.Services.Casos;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [Route("api/casos")]
    public class CasosController : ApiControllerBase
    {
        private readonly ICasosService _client;
        private readonly IBedrockService _bedrockService;

        public CasosController(ICasosService casosService, IBedrockService bedrockService)
        {
            _client = casosService;
            _bedrockService = bedrockService;
        }
        ///<summary>
        ///Realiza uma consulta à API do Verde utilizando o número do cpf para retornar os casos do assistido.
        ///</summary>
        ///<remarks>
        ///Parâmetro: cpf. 
        ///O endpoint aceita o número do cpf em um dos seguintes formatos:
        ///00000000000
        ///000.000.000-00
        /// </remarks>
        ///<param name="cpf">Número do cpf do assistido</param>
        ///<returns>Dados de casos do assistido</returns>
        [HttpGet("{cpf}")]
        [ProducesResponseType(typeof(Models.CasosDTO.RespostaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(string cpf, int offset = 0)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return ErroParametroInvalido("Cpf do assistido obrigatório");
            }

            try
            {
                var processo = await _client.GetCasosAsync(cpf, offset, 5);

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
