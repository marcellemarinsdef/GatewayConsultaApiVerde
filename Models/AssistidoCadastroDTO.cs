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
    // dtNascimento incluído (maria-ia#20260202): o PUT do Verde trata campo
    // ausente como "apagar", não "manter" — sem reenviar a data atual, toda
    // atualização de endereço/telefone/email zerava a data de nascimento
    // da pessoa no cadastro real da Defensoria.
    public class AtualizarAssistidoRequestDTO
    {
        [JsonPropertyName("endereco")]
        public EnderecoAssistidoDTO Endereco { get; set; }
        [JsonPropertyName("telefone")]
        public TelefoneAssistidoDTO Telefone { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("dtNascimento")]
        public string DtNascimento { get; set; }
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
        [JsonPropertyName("dtNascimento")]
        public string DtNascimento { get; set; }
    }
}
