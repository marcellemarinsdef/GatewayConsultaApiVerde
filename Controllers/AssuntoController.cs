using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.Assunto;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    // Card Coilab #20260185 — identifica o idAssunto real do Verde a partir
    // do relato do assistido (mesma taxonomia usada por Órgão/Agendamento/
    // Encaminhamento, cards #20260146/#20260147).
    [Route("api/assunto")]
    public class AssuntoController : ApiControllerBase
    {
        private readonly IAssuntoService _client;

        public AssuntoController(IAssuntoService assuntoService)
        {
            _client = assuntoService;
        }

        ///<summary>Lista as categorias de assunto disponíveis no Verde.</summary>
        [HttpGet("categorias")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Categorias()
        {
            try
            {
                return Ok(await _client.GetCategoriasAsync());
            }
            catch (TimeoutRejectedException) { return ErroTimeout("Tempo limite excedido ao consultar categorias."); }
            catch (ApiException ex) { return ErroApi(ex); }
        }

        ///<summary>Consulta um item da árvore de assuntos (por categoria ou por item específico).</summary>
        [HttpGet("consultar-item-arvore")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarItemArvore([FromQuery] int? idCategoria, [FromQuery] int? idItemCategoria)
        {
            if (!idCategoria.HasValue && !idItemCategoria.HasValue)
                return ErroParametroInvalido("Informe idCategoria ou idItemCategoria");
            try
            {
                return Ok(await _client.ConsultarItemArvoreAsync(idCategoria, idItemCategoria));
            }
            catch (TimeoutRejectedException) { return ErroTimeout("Tempo limite excedido ao consultar item da árvore."); }
            catch (ApiException ex) { return ErroApi(ex); }
        }

        ///<summary>Sugere motivos (categorias) prováveis a partir de um relato textual livre (IA do Verde).</summary>
        ///<param name="dados">relato do assistido, codificado em base64 (contrato do Verde)</param>
        [HttpPost("sugerir-motivo")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> SugerirMotivo([FromBody] RelatoRequestDTO dados)
        {
            if (string.IsNullOrWhiteSpace(dados?.Relato))
                return ErroParametroInvalido("relato obrigatório");
            try
            {
                var (statusCode, body) = await _client.SugerirMotivoAsync(dados);
                return StatusCode(statusCode, body);
            }
            catch (TimeoutRejectedException) { return ErroTimeout("Tempo limite excedido ao sugerir motivo."); }
            catch (ApiException ex) { return ErroApi(ex); }
        }

        ///<summary>Consulta os dados de um assunto pelo id.</summary>
        [HttpGet("{idAssunto}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int idAssunto)
        {
            try
            {
                return Ok(await _client.GetAssuntoAsync(idAssunto));
            }
            catch (TimeoutRejectedException) { return ErroTimeout("Tempo limite excedido ao consultar assunto."); }
            catch (ApiException ex) { return ErroApi(ex); }
        }
    }
}
