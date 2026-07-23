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
    [ApiController]
    [Route("api/agendamento")]
    public class AgendamentoController : Controller
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
        public async Task<IActionResult> Get(int idEvento)
        {
            try
            {
                var agendamento = await _client.GetDetalheAsync(idEvento);
                return Ok(agendamento);
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao consultar o agendamento."
                );
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        ///<summary>
        ///Consulta as vagas disponíveis para reagendamento de um evento.
        ///</summary>
        ///<param name="idEvento">Id do evento (agendamento) no Verde</param>
        [HttpGet("vagas/{idEvento}")]
        public async Task<IActionResult> GetVagas(int idEvento)
        {
            try
            {
                var vagas = await _client.GetVagasAsync(idEvento);
                return Ok(vagas);
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao consultar vagas."
                );
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        ///<summary>
        ///Reagenda um agendamento existente para uma nova data.
        ///</summary>
        ///<param name="dados">idAgendamento, configuracaoIntervaloAgenda e dataNova</param>
        [HttpPost("reagendar")]
        public async Task<IActionResult> Reagendar([FromBody] ReagendarRequestDTO dados)
        {
            try
            {
                var (statusCode, body) = await _client.ReagendarAsync(dados);
                return StatusCode(statusCode, body);
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao reagendar."
                );
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
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
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao desmarcar."
                );
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }
}
