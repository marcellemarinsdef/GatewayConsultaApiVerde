using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class DesmarcarRequestDTO
    {
        [JsonPropertyName("idAgendamento")]
        public int IdAgendamento { get; set; }
        [JsonPropertyName("idPessoa")]
        public int IdPessoa { get; set; }
    }
}
