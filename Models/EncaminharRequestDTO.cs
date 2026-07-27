using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    // "Primeiro atendimento" sem hora marcada — issue #39, cards Coilab
    // #20260147/#20260185. Espelha EncaminhamentoDTO do Verde real
    // (POST /integra/encaminhamento/encaminhar, confirmado no Swagger oficial).
    public class EncaminharRequestDTO
    {
        [JsonPropertyName("idPessoa")]
        public int IdPessoa { get; set; }
        [JsonPropertyName("idOrgao")]
        public int IdOrgao { get; set; }
        [JsonPropertyName("idLocalAtendimento")]
        public int? IdLocalAtendimento { get; set; } // só quando Presencial
        [JsonPropertyName("urgencia")]
        public bool Urgencia { get; set; }
        [JsonPropertyName("idAssunto")]
        public int IdAssunto { get; set; }
        // "Remoto" | "Presencial" (enum do Verde)
        [JsonPropertyName("preferenciaAtendimento")]
        public string PreferenciaAtendimento { get; set; }
        [JsonPropertyName("idCaso")]
        public int? IdCaso { get; set; }
        [JsonPropertyName("idProcesso")]
        public int? IdProcesso { get; set; }
        [JsonPropertyName("idPessoaPresa")]
        public int? IdPessoaPresa { get; set; }
        // "PRIMEIRO_ATENDIMENTO" | "PRISIONAL" | "PROCESSUAL" | "VIOLENCIA_DOMESTICA"
        [JsonPropertyName("fluxoEncaminhamento")]
        public string FluxoEncaminhamento { get; set; } = "PRIMEIRO_ATENDIMENTO";
        [JsonPropertyName("motivoUrgencia")]
        public string MotivoUrgencia { get; set; }
        [JsonPropertyName("dataComplemento")]
        public string DataComplemento { get; set; }
        [JsonPropertyName("textoComplemento")]
        public string TextoComplemento { get; set; }
        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }
    }
}
