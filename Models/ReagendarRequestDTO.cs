using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class ReagendarRequestDTO
    {
        [JsonPropertyName("idAgendamento")]
        public int IdAgendamento { get; set; }
        [JsonPropertyName("configuracaoIntervaloAgenda")]
        public int ConfiguracaoIntervaloAgenda { get; set; }
        [JsonPropertyName("dataNova")]
        public string DataNova { get; set; }
    }
}
