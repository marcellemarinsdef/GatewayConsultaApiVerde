using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Models.Responses;
using GatewayConsultaApiVerde.Services.Bedrock;
using GatewayConsultaApiVerde.Services.Processo;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;

namespace GatewayConsultaApiVerde.Controllers
{
    [Route("api/processo")]
    public class ProcessoController : ApiControllerBase
    {
        private readonly IProcessoService _processoService;
        private readonly IBedrockService _bedrockService;

        public ProcessoController(IProcessoService processoService, IBedrockService bedrockService)
        {
            _processoService = processoService;
            _bedrockService = bedrockService;
        }
        ///<summary>
        ///Realiza uma consulta à API do Verde utilizando o número do processo informado pelo usuário.
        ///</summary>
        ///<remarks>
        ///Parâmetro: numeroProcesso. 
        ///O endpoint aceita o número do processo em um dos seguintes formatos:
        ///00000000000000000000
        ///0000000-00.0000.0.00.0000 
        /// </remarks>
        ///<param name="numeroProcesso">Número do processo no verde</param>
        ///<returns>Movimento do processo</returns>
        [HttpGet("{numeroProcesso}")]
        [ProducesResponseType(typeof(ProcessoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Get(string numeroProcesso)
        {
            if (string.IsNullOrWhiteSpace(numeroProcesso))
            {
                return ErroParametroInvalido("Número do processo obrigatório");
            }

            try
            {
                var processo = await _processoService.GetProcessoAsync(numeroProcesso);

                return Ok(processo);
            }
            catch (TimeoutRejectedException)
            {
                return ErroTimeout("Tempo limite excedido ao consultar o processo.");
            }
            catch (ApiException ex)
            {
                return ErroApi(ex);
            }
        }

        [HttpGet("simplificar/{numero}")]
        public async Task<IActionResult> GetProcessoLinguagemSimples(string numero)
        {
            var processo =
                await _processoService.GetProcessoAsync(numero);


            var json =
                processo.RootElement.GetRawText();


            var prompt = $"""
            Você é um assistente que explica processos judiciais para cidadãos.

            Regras:
            - Responda em português do Brasil, com linguagem simples e clara.
            - Evite termos jurídicos; quando necessários, explique-os brevemente.
            - Seja objetivo e não repita informações.
            - Baseie-se apenas nos dados fornecidos. Não invente, suponha ou preveja informações.
            - Não forneça aconselhamento jurídico.
            - Só informe ações que o cidadão deve realizar se elas estiverem explicitamente indicadas.
            - Diferencie fatos já ocorridos de possíveis próximos passos.
            - Se não houver informação suficiente sobre o próximo passo, informe isso.
            - Não use títulos, subtítulos ou marcadores para organizar a resposta, exceto "Assunto:" e "Movimentações:".
            - Apenas liste a data das 3 movimentações mais recentes, da mais recente para a mais antiga.
            - Não explique, interprete ou resuma as movimentações. 
            - Não escreva caracteres de quebra de linha escapados como "\n". Use quebras de linha reais quando necessário.
            - Não escreva conclusão ou texto adicional.


            informe: 
            
            - Assunto: [tipo] . [Explique o estado do processo e em que órgão ele está]. Movimentações: [datas]. [Explique em até 2 frases o próximo passo apenas se houver informação suficiente.]

            - Se não houver providência para o cidadão, escreva:
              "Não há nenhuma ação necessária da sua parte."

            

            Dados do processo:

            {json}
            """;


            var resposta =
                await _bedrockService.GerarRespostaAsync(prompt);

            return Ok(new
            {
                explicacao = resposta
            });
        }
    }
}
