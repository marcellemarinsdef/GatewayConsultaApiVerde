using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Agendamento;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // Rota singular ("agendamento") — evita colisão com a rota existente
    // GET api/agendamentos/{cpf} (AgendamentosController), que casaria com
    // {idEvento} se ficasse no mesmo controller.
    [Route("api/agendamento")]
    public class AgendamentoController : ApiControllerBase
    {
        private readonly IAgendamentoService _client;

        public AgendamentoController(IAgendamentoService agendamentoService)
        {
            _client = agendamentoService;
        }

        ///<summary>
        ///Consulta o detalhe de um agendamento pelo id do evento.
        ///</summary>
        ///<param name="idEvento">Id do evento (agendamento) no Verde</param>
        [HttpGet("{idEvento}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(int idEvento)
        {
            try
            {
                var agendamento = await _client.GetDetalheAsync(idEvento);
                return Ok(agendamento);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar o agendamento.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }

        ///<summary>
        ///Consulta as vagas disponíveis para reagendamento de um evento.
        ///</summary>
        ///<param name="idEvento">Id do evento (agendamento) no Verde</param>
        [HttpGet("vagas/{idEvento}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> GetVagas(int idEvento)
        {
            try
            {
                var vagas = await _client.GetVagasAsync(idEvento);
                return Ok(vagas);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar vagas.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }

        ///<summary>
        ///Reagenda um agendamento existente para uma nova data.
        ///</summary>
        ///<param name="dados">idAgendamento, configuracaoIntervaloAgenda e dataNova</param>
        [HttpPost("reagendar")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Reagendar([FromBody] ReagendarRequestDTO dados)
        {
            try
            {
                var (statusCode, body) = await _client.ReagendarAsync(dados);
                return StatusCode(statusCode, body);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao reagendar.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }

        ///<summary>
        ///Desmarca um agendamento existente.
        ///</summary>
        ///<param name="dados">idAgendamento e idPessoa</param>
        ///<remarks>
        ///Retorna 204 quando o agendamento é desmarcado e o e-mail de
        ///confirmação é enviado com sucesso; 200 quando desmarca mas o envio
        ///do e-mail falha (sucesso parcial).
        ///</remarks>
        [HttpPost("desmarcar")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Desmarcar([FromBody] DesmarcarRequestDTO dados)
        {
            try
            {
                var (statusCode, body) = await _client.DesmarcarAsync(dados);
                return statusCode == StatusCodes.Status204NoContent
                    ? NoContent()
                    : Ok(body);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao desmarcar.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }
    }
}
