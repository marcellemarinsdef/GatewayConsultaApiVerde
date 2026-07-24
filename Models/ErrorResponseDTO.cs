using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    // Envelope de erro padrão (padroes-rest-nivel2) — {error:{code,message}}.
    // code é estável entre releases (pro client tratar por código);
    // message é texto pro humano, pode mudar.
    public class ErrorResponseDTO
    {
        [JsonPropertyName("error")]
        public ErrorDetail Error { get; set; }

        public class ErrorDetail
        {
            [JsonPropertyName("code")]
            public string Code { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
