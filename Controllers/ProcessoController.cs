using ApiConsultaProcesso.Services;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace ApiConsultaProcesso.Controllers
{
    [ApiController]
    [Route("api/processos")]
    public class ProcessoController : Controller
    {
        private readonly IProcessoService _processoService;

        public ProcessoController(IProcessoService processoService)
        {
            _processoService = processoService;
        }

        [HttpGet("{numeroProcesso}")]
        public async Task<IActionResult> Get(string numeroProcesso)
        {
            if (string.IsNullOrWhiteSpace(numeroProcesso))
            {
                return BadRequest("Número do processo obrigatório");
            }

            try
            {
                var processo = await _processoService.GetProcessoAsync(numeroProcesso);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Tempo limite excedido ao consultar o processo."
                );
            }
        }
    }
}
