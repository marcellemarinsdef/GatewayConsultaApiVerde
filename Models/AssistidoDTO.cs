using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class AssistidoDTO
    {
        public class RespostaDTO
        {

            [JsonPropertyName("dados")]
            public Assistido Dados { get; set; }
        }

        public class Assistido
        {
            [JsonPropertyName("idPessoa")]
            public int Id { get; set; }
        }
        
    }
}
