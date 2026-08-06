using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Agendamento
{
    // Detalhe/vagas/reagendar/desmarcar operam direto sobre idEvento/idAgendamento
    // (já conhecido pelo MarIA a partir da listagem de AgendamentosController) —
    // sem indireção por cpf->idPessoa como em Casos/Agendamentos (issue #21).
    public class AgendamentoService : IAgendamentoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public AgendamentoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<JsonDocument> GetDetalheAsync(int idEvento) =>
            _consultaVerdeClient.GetAsync($"agendamento/{idEvento}");

        public Task<JsonDocument> GetVagasAsync(int idEvento) =>
            _consultaVerdeClient.GetAsync($"agendamento/consultar-vagas/{idEvento}");

        public Task<(int StatusCode, JsonDocument? Body)> ReagendarAsync(ReagendarRequestDTO dados) =>
            _consultaVerdeClient.PostAsync("agendamento/reagendar", dados);

        public Task<(int StatusCode, JsonDocument? Body)> DesmarcarAsync(DesmarcarRequestDTO dados) =>
            _consultaVerdeClient.PostAsync("agendamento/desmarcar", dados);

        public Task<(int StatusCode, JsonDocument? Body)> AgendarAsync(AgendarRequestDTO dados) =>
            _consultaVerdeClient.PostAsync("agendamento/agendar", dados);

        public Task<JsonDocument> VerificarDuplicadosAsync(int idPessoa, int idAssunto) =>
            _consultaVerdeClient.GetAsync($"agendamento/verificar-duplicados?idPessoa={idPessoa}&idAssunto={idAssunto}");
    }
}
