using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;
using System.Web;

namespace GatewayConsultaApiVerde.Services.Orgao
{
    public class OrgaoService : IOrgaoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public OrgaoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<JsonDocument> GetPrimeiroAtendimentoAsync(int idPessoa, int idAssunto, string complemento)
        {
            var endpoint = $"orgao/primeiro-atendimento?idPessoa={idPessoa}&idAssunto={idAssunto}";
            if (!string.IsNullOrWhiteSpace(complemento))
                endpoint += $"&complemento={HttpUtility.UrlEncode(complemento)}";
            return _consultaVerdeClient.GetAsync(endpoint);
        }
    }
}
