using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    // "Primeiro atendimento" com hora marcada — issue #39, cards Coilab
    // #20260146/#20260185. Espelha RealizarAgendamentoDTO do Verde real
    // (POST /integra/agendamento/agendar, confirmado no Swagger oficial).
    public class AgendarRequestDTO
    {
        [JsonPropertyName("idPessoa")]
        public int IdPessoa { get; set; }
        [JsonPropertyName("idOrgao")]
        public int IdOrgao { get; set; }
        [JsonPropertyName("idIntervalo")]
        public int IdIntervalo { get; set; }
        [JsonPropertyName("idAssunto")]
        public int IdAssunto { get; set; }
        [JsonPropertyName("idPessoaPresa")]
        public int? IdPessoaPresa { get; set; }
        [JsonPropertyName("idProcesso")]
        public int? IdProcesso { get; set; }
        [JsonPropertyName("resumoAtendimento")]
        public string ResumoAtendimento { get; set; }
        // "PRIMEIRO_ATENDIMENTO" | "PRISIONAL" (enum do Verde)
        [JsonPropertyName("fluxoAtendimento")]
        public string FluxoAtendimento { get; set; } = "PRIMEIRO_ATENDIMENTO";
        [JsonPropertyName("dataVaga")]
        public string DataVaga { get; set; }
        [JsonPropertyName("dataComplemento")]
        public string DataComplemento { get; set; }
        [JsonPropertyName("textoComplemento")]
        public string TextoComplemento { get; set; }
    }
}
