using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Orgao;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // issue #39, cards Coilab #20260146/#20260147 — consulta o(s) órgão(s)
    // responsável(is) pelo primeiro atendimento de um assunto. O campo
    // tipoAtendimento na resposta decide se o fluxo segue por Agendamento
    // (hora marcada) ou Encaminhamento (sem hora).
    [Route("api/orgao")]
    public class OrgaoController : ApiControllerBase
    {
        private readonly IOrgaoService _client;

        public OrgaoController(IOrgaoService orgaoService)
        {
            _client = orgaoService;
        }

        ///<summary>
        ///Consulta o(s) órgão(s) responsável(is) pelo primeiro atendimento de um assunto.
        ///</summary>
        ///<param name="idPessoa">Id da pessoa no Verde (resolvido a partir do CPF)</param>
        ///<param name="idAssunto">Id do assunto/motivo (ex: vindo de um caso já consultado)</param>
        ///<param name="complemento">Complemento textual opcional do motivo</param>
        [HttpGet("primeiro-atendimento")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> PrimeiroAtendimento([FromQuery] int idPessoa, [FromQuery] int idAssunto, [FromQuery] string? complemento)
        {
            if (idPessoa <= 0 || idAssunto <= 0)
            {
                return ErroParametroInvalido("idPessoa e idAssunto obrigatórios");
            }

            try
            {
                var orgaos = await _client.GetPrimeiroAtendimentoAsync(idPessoa, idAssunto, complemento);
                return Ok(orgaos);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar o órgão de atendimento.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
