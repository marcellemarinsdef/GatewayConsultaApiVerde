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
        public async Task<IActionResult> Get(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return ErroParametroInvalido("Cpf do assistido obrigatório");
            }

            try
            {
                var processo = await _client.GetCasosAsync(cpf);

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
        [HttpGet("simplificar/{cpf}")]
        public async Task<IActionResult> GetProcessoLinguagemSimples(string cpf)
        {
            var processo =
                await _client.GetCasosAsync(cpf);


            var json =
                processo.Dados;


            var prompt = $"""
            Você é um assistente de informações processuais.

            Sua função é explicar o andamento de processos judiciais para cidadãos que não possuem conhecimento jurídico.

            Seu objetivo é fazer com que qualquer pessoa consiga entender rapidamente o que está acontecendo no processo.

            REGRAS:

            - Responda em português do Brasil.
            - Use linguagem simples, clara e direta.
            - Escreva como se estivesse explicando o processo para uma pessoa comum.
            - Evite termos jurídicos. Quando forem necessários, explique-os de forma simples.
            - Seja breve. Não repita informações.
            - Não invente informações.
            - Use somente os dados fornecidos sobre o processo e as informações relevantes encontradas na base de conhecimento.
            - Não presuma o que aconteceu ou o que irá acontecer.
            - Não faça previsões sobre o resultado do processo.
            - Não invente prazos.
            - Não diga que a pessoa precisa fazer algo se isso não estiver indicado nos dados.
            - Diferencie claramente o que já aconteceu do que pode acontecer.
            - Se não houver informação suficiente para saber o próximo passo, diga isso de forma simples.
            - Não forneça aconselhamento jurídico.

            FORMATO DA RESPOSTA:

            ### Situação atual

            Explique em no máximo 2 frases:
            - onde o processo está;
            - qual é o assunto, se estiver disponível;
            - qual foi a última movimentação relevante.

            ### Últimas atualizações
            
            Mostre somente as 2 ou 3 movimentações mais recentes e relevantes.
            Monte uma linha do tempo coesa, de forma que a ordem das movimentações seja clara. 

            Use o formato:

            - DD/MM/AAAA: explicação simples da movimentação.

            ### O que acontece agora?

            Explique em no máximo 2 frases informações relevantes sobre os casos.

            Se não houver nenhuma providência indicada para o cidadão, informe:

            "No momento, não há nenhuma ação necessária da sua parte."

            IMPORTANTE:

            Não escreva uma conclusão ou resumo adicional depois dessas três seções.

            DADOS DO PROCESSO:

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
