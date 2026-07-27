using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Bloqueio
{
    public class BloqueioService : IBloqueioService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public BloqueioService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<JsonDocument> GetBloqueioAsync(int idPessoa, int idAssunto, int idOrgao) =>
            _consultaVerdeClient.GetAsync($"bloqueio?idPessoa={idPessoa}&idAssunto={idAssunto}&idOrgao={idOrgao}");
    }
}
