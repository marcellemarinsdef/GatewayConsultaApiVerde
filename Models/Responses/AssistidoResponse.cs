using System;
using System.Collections.Generic;

namespace GatewayConsultaApiVerde.Models.Responses
{
    public class AssistidoResponse
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public PessoaDto Dados { get; set; } = new();
    }

    public class PessoaDto
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeSocial { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public EnderecoDetalhadoDto EnderecoDetalhado { get; set; } = new();
        public string Email { get; set; } = string.Empty;
        public bool PossuiContaAtivaAplicativo { get; set; }
        public string StatusValidacao { get; set; } = string.Empty;
        public string StatusVerificacaoEmail { get; set; } = string.Empty;
        public List<TelefoneAssistidoDto> TelefonesAssistido { get; set; } = new();
    }

    public class EnderecoDetalhadoDto
    {
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
        public DateTime? DataEndereco { get; set; }
    }

    public class TelefoneAssistidoDto
    {
        public int Id { get; set; }
        public string NumeroTelefone { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
        public bool InWhatsapp { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Ramal { get; set; } = string.Empty;
        public DateTime? DataIndicacaoWhatsapp { get; set; }
    }
}
