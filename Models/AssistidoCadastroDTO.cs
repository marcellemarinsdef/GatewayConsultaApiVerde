using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class EnderecoAssistidoDTO
    {
        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }
        [JsonPropertyName("numero")]
        public string Numero { get; set; }
        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }
        [JsonPropertyName("cep")]
        public string Cep { get; set; }
        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }
        [JsonPropertyName("municipio")]
        public string Municipio { get; set; }
        [JsonPropertyName("uf")]
        public string Uf { get; set; }
    }

    public class TelefoneAssistidoDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("numeroTelefone")]
        public string NumeroTelefone { get; set; }
        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }
        [JsonPropertyName("inWhatsapp")]
        public bool InWhatsapp { get; set; }
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }
        [JsonPropertyName("ramal")]
        public string Ramal { get; set; }
        [JsonPropertyName("dataIndicacaoWhatsapp")]
        public string DataIndicacaoWhatsapp { get; set; }
    }

    // Corpo recebido do maria-ia E enviado ao Verde sem transformação — cadastro
    // não precisa de idPessoa (o Verde gera), então não há divergência de shape.
    public class CadastrarAssistidoRequestDTO
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
        [JsonPropertyName("nomeSocial")]
        public string NomeSocial { get; set; }
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
        [JsonPropertyName("endereco")]
        public EnderecoAssistidoDTO Endereco { get; set; }
        [JsonPropertyName("telefones")]
        public List<TelefoneAssistidoDTO> Telefones { get; set; } = new List<TelefoneAssistidoDTO>();
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("dtNascimento")]
        public string DtNascimento { get; set; }
    }

    // Corpo recebido do maria-ia — só o que ele sabe (cpf já vai na URL, não
    // no corpo). idPessoa é resolvido no servidor antes de chamar o Verde.
    //
    // dtNascimento NÃO entra aqui de propósito (revertido 2026-08-05,
    // maria-ia#20260202): tentamos reenviar a data atual pra evitar que o
    // PUT zerasse esse campo (sintoma real observado), mas o Verde rejeita
    // o payload inteiro com "Propriedade não reconhecida: 'dtNascimento'"
    // (400) quando esse campo aparece aqui — esse PUT específico não
    // suporta esse campo de jeito nenhum, nem pra manter o valor atual.
    // Limitação do lado do Verde, reportar pro time da Defensoria.
    public class AtualizarAssistidoRequestDTO
    {
        [JsonPropertyName("endereco")]
        public EnderecoAssistidoDTO Endereco { get; set; }
        [JsonPropertyName("telefone")]
        public TelefoneAssistidoDTO Telefone { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    // Corpo efetivamente enviado ao Verde (PUT /integra/pessoa) — com idPessoa
    // resolvido a partir do cpf (issue #31: Verde identifica por idPessoa, não cpf).
    public class AtualizarPessoaVerdeDTO
    {
        [JsonPropertyName("idPessoa")]
        public int IdPessoa { get; set; }
        [JsonPropertyName("endereco")]
        public EnderecoAssistidoDTO Endereco { get; set; }
        [JsonPropertyName("telefone")]
        public TelefoneAssistidoDTO Telefone { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
