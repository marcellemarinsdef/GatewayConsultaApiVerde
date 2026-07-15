using System.Text.Json.Serialization;

namespace ApiConsultaProcesso.Models
{
    public class MovimentosDTO
    {
        [JsonPropertyName("titulo")]
        public string Titulo {  get; set; }
        [JsonPropertyName("data")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Data { get; set; }
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
        [JsonPropertyName("traducao")]
        public string traducao { get; set; }
    }
}
