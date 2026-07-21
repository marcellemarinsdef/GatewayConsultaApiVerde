using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class PessoaDTO
    {
        public class RespostaDTO
        {
            [JsonPropertyName("dados")]
            public Pessoa Dados { get; set; }
        }
        public class Pessoa
        {
            [JsonPropertyName("idPessoa")]
            public int Id { get; set; }
        }
    }
}
