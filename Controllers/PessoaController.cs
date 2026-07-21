using GatewayConsultaApiVerde.Services;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [ApiController]
    [Route("api/pessoa")]
    public class PessoaController : Controller
    {
        private readonly IVerdeApiClient _client;

        public PessoaController(IVerdeApiClient pessoaService)
        {
            _client = pessoaService;
        }

        [HttpGet("{cpfPessoa}")]
        public async Task<IActionResult> Get(string cpfPessoa)
        {
            if (string.IsNullOrWhiteSpace(cpfPessoa))
            {
                return BadRequest("Número do processo obrigatório");
            }

            try
            {
                var processo = await _client.GetPessoaAsync(cpfPessoa);

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
