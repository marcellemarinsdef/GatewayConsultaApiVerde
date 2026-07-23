using System.Text.Json.Serialization;

namespace GatewayConsultaApiVerde.Models
{
    public class CasosDTO
    {
    
    public class RespostaDto
    {
        [JsonPropertyName("dados")]
        public List<CasoDto> Dados { get; set; }
    }

    public class CasoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("orgaosAssociados")]
        public List<OrgaoAssociadoDto> OrgaosAssociados { get; set; }

        [JsonPropertyName("dataAtualizacao")]
        public string DataAtualizacao { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("tipoCaso")]
        public string TipoCaso { get; set; }

        [JsonPropertyName("assunto")]
        public AssuntoDto Assunto { get; set; }

        [JsonPropertyName("idProcesso")]
        public int IdProcesso { get; set; }

        [JsonPropertyName("relacaoAssistido")]
        public string RelacaoAssistido { get; set; }

        [JsonPropertyName("andamentos")]
        public List<AndamentoDto> Andamentos { get; set; }

        [JsonPropertyName("numeroProcesso")]
        public string NumeroProcesso { get; set; }
    }

    public class OrgaoAssociadoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("enderecos")]
        public List<EnderecoDto> Enderecos { get; set; }

        [JsonPropertyName("responsavel")]
        public bool Responsavel { get; set; }
    }

    public class EnderecoDto
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

        [JsonPropertyName("idLocalAtendimento")]
        public int IdLocalAtendimento { get; set; }

        [JsonPropertyName("horariosAtendimento")]
        public List<HorarioDto> HorariosAtendimento { get; set; }

        [JsonPropertyName("horariosUrgencia")]
        public List<HorarioDto> HorariosUrgencia { get; set; }

        [JsonPropertyName("horariosAtendimentoFormatado")]
        public string HorariosAtendimentoFormatado { get; set; }

        [JsonPropertyName("horariosUrgenciaFormatado")]
        public string HorariosUrgenciaFormatado { get; set; }
    }

    public class HorarioDto
    {
        [JsonPropertyName("diaDaSemana")]
        public string DiaDaSemana { get; set; }

        [JsonPropertyName("horaInicio")]
        public string HoraInicio { get; set; }

        [JsonPropertyName("horaFim")]
        public string HoraFim { get; set; }

        [JsonPropertyName("informacaoComplementar")]
        public string InformacaoComplementar { get; set; }
    }

    public class AssuntoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }

    public class AndamentoDto
    {
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}
}
