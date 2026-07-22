using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class AssistidoDTO
    {
        public class RespostaDTO
        {
            [JsonPropertyName("codigo")]
            public string? Codigo { get; set; }

            [JsonPropertyName("mensagem")]
            public string? Mensagem { get; set; }

            [JsonPropertyName("dados")]
            public Assistido? Dados { get; set; }
        }

        public class Assistido
        {
            [JsonPropertyName("idPessoa")]
            public int Id { get; set; }

            [JsonPropertyName("nome")]
            public string? Nome { get; set; }

            [JsonPropertyName("nomeSocial")]
            public string? NomeSocial { get; set; }

            [JsonPropertyName("endereco")]
            public string? Endereco { get; set; }

            [JsonPropertyName("enderecoDetalhado")]
            public EnderecoDetalhado? EnderecoDetalhado { get; set; }

            [JsonPropertyName("email")]
            public string? Email { get; set; }

            [JsonPropertyName("possuiContaAtivaAplicativo")]
            public bool PossuiContaAtivaAplicativo { get; set; }

            [JsonPropertyName("statusValidacao")]
            public string? StatusValidacao { get; set; }

            [JsonPropertyName("statusVerificacaoEmail")]
            public string? StatusVerificacaoEmail { get; set; }

            [JsonPropertyName("telefonesAssistido")]
            public List<TelefoneAssistido> TelefonesAssistido { get; set; } = [];
        }

        public class EnderecoDetalhado
        {
            [JsonPropertyName("logradouro")]
            public string? Logradouro { get; set; }

            [JsonPropertyName("numero")]
            public string? Numero { get; set; }

            [JsonPropertyName("complemento")]
            public string? Complemento { get; set; }

            [JsonPropertyName("bairro")]
            public string? Bairro { get; set; }

            [JsonPropertyName("municipio")]
            public string? Municipio { get; set; }

            [JsonPropertyName("uf")]
            public string? Uf { get; set; }

            [JsonPropertyName("cep")]
            public string? Cep { get; set; }

            [JsonPropertyName("referencia")]
            public string? Referencia { get; set; }

            [JsonPropertyName("dataEndereco")]
            public string? DataEndereco { get; set; }
        }

        public class TelefoneAssistido
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("numeroTelefone")]
            public string? NumeroTelefone { get; set; }

            [JsonPropertyName("observacao")]
            public string? Observacao { get; set; }

            [JsonPropertyName("inWhatsapp")]
            public bool InWhatsapp { get; set; }

            [JsonPropertyName("tipo")]
            public string? Tipo { get; set; }

            [JsonPropertyName("ramal")]
            public string? Ramal { get; set; }

            [JsonPropertyName("dataIndicacaoWhatsapp")]
            public DateTime? DataIndicacaoWhatsapp { get; set; }
        }
    }
}
