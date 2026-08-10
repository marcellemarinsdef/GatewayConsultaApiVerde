using System.Text.Json.Serialization;
using static GatewayConsultaApiVerde.Models.CasosDTO;

namespace GatewayConsultaApiVerde.Models
{
    public class CasosPaginacaoDTO
    {
        [JsonPropertyName("dados")]
        public List<CasoDto> Dados { get; set; } = [];

        public class CasoDto
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("numeroProcesso")]
            public string NumeroProcesso { get; set; }
        }

        public int TotalCasos { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public bool TemMais { get; set; }
        public int ProximoOffset { get; set; }
    }
}
