using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class AgendamentosDTO
    {
        public class RespostaAgendamentosDTO
        {
            [JsonPropertyName("dados")]
            public DadosDTO Dados { get; set; }
        }


        public class DadosDTO
        {
            [JsonPropertyName("agendamentos")]
            public List<AgendamentoDTO> Agendamentos { get; set; }
        }

        public class AgendamentoDTO
        {
            [JsonPropertyName("numeroAgendamento")]
            public int? NumeroAgendamento { get; set; }
            [JsonPropertyName("status")]
            public string? Status { get; set; }
            [JsonPropertyName("dataAgendamento")]
            public string? DataAgendamento { get; set; }
            [JsonPropertyName("horaAgendada")]
            public string? HoraAgendada { get; set; }
            [JsonPropertyName("orgao")]
            public OrgaoDto? Orgao { get; set; }
            [JsonPropertyName("assunto")]
            public AssuntoDto? AssuntoDTO { get; set; }
            [JsonPropertyName("tipoPauta")]
            public string? TipoPauta { get; set; }
            [JsonPropertyName("idCaso")]
            public int? IdCaso { get; set; }
            [JsonPropertyName("presencaConfirmada")]
            public bool? PresencaConfirmada { get; set; }
        }

        public class OrgaoDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("nome")]
            public string? Nome { get; set; }
            [JsonPropertyName("enderecos")]
            public List<EnderecoDto>? Enderecos { get; set; }       
        }

        public class EnderecoDto
        {
            [JsonPropertyName("logradouro")]
            public string? Logradouro { get; set; }
            [JsonPropertyName("numero")]
            public string? Numero { get; set; }
            [JsonPropertyName("complemento")]
            public string? Complemento { get; set; }
            [JsonPropertyName("cep")]
            public string? Cep { get; set; }
            [JsonPropertyName("bairro")]
            public string? Bairro { get; set; }
            [JsonPropertyName("municipio")]
            public string? Municipio { get; set; }
            [JsonPropertyName("uf")]
            public string? Uf { get; set; }
        }

        public class AssuntoDto
        {
            [JsonPropertyName("id")]
            public int? Id { get; set; }
            [JsonPropertyName("nome")]
            public string? Nome { get; set; }
            [JsonPropertyName("descricao")]
            public string? Descricao { get; set; }
        }
    }


}