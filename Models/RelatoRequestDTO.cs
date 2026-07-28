using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    // POST /integra/assunto/sugerir-motivo — issue #39/card #20260185.
    // `relato` deve vir codificado em base64 (contrato do Verde).
    public class RelatoRequestDTO
    {
        [JsonPropertyName("relato")]
        public string? Relato { get; set; }
    }
}
