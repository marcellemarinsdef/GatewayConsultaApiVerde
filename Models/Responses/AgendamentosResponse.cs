using System;
using System.Collections.Generic;

namespace GatewayConsultaApiVerde.Models.Responses
{
        public class AgendamentosResponse
        {
            public int NumeroAgendamento { get; set; }
            public string Status { get; set; }
            public string SistemaOrigem { get; set; }
            public string DataAgendamento { get; set; }
            public string DataRealizacaoAgendamento { get; set; }
            public string HoraAgendada { get; set; }
            public Orgao Orgao { get; set; }
            public Assunto Assunto { get; set; }
            public string TipoPauta { get; set; }
            public int IdCaso { get; set; }
            public int IdPessoa { get; set; }
            public bool BloqueioAtivo { get; set; }
            public bool PresencaConfirmada { get; set; }
        }

        public class Orgao
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public List<Endereco> Enderecos { get; set; }
            public List<Telefone> Telefones { get; set; }
            public List<Emails> Emails { get; set; }
            public List<HorarioAtendimento> HorariosAtendimento { get; set; }
            public string TipoAtendimento { get; set; }
            public bool AtendeCRC { get; set; }
            public List<string> TiposOrgao { get; set; }
            public bool Responsavel { get; set; }
            public VagasDisponiveisDTO VagasDisponiveis { get; set; }
        }

        public class Endereco
        {
            public string Logradouro { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }
            public string Cep { get; set; }
            public string Bairro { get; set; }
            public string Municipio { get; set; }
            public string Uf { get; set; }
            public int IdLocalAtendimento { get; set; }
            public List<HorarioAtendimento> HorariosAtendimento { get; set; }
            public List<HorarioAtendimento> HorariosUrgencia { get; set; }
            public string HorariosAtendimentoFormatado { get; set; }
            public string HorariosUrgenciaFormatado { get; set; }
        }

        public class HorarioAtendimento
        {
            public string DiaDaSemana { get; set; }
            public DateTime HoraInicio { get; set; }
            public DateTime HoraFim { get; set; }
            public string InformacaoComplementar { get; set; }
        }

        public class Telefone
        {
            public int Id { get; set; }
            public string NumeroTelefone { get; set; }
            public string Observacao { get; set; }
            public bool InWhatsapp { get; set; }
            public string Tipo { get; set; }
            public string Ramal { get; set; }
            public string DataIndicacaoWhatsapp { get; set; }
        }

        public class Emails
        {
            public string Email { get; set; }
            public string Observacao { get; set; }
        }

        public class Assunto
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string NomeMateria { get; set; }
            public string Descricao { get; set; }
            public string TxDocumentosNecessarios { get; set; }
            public bool Urgente { get; set; }
            public bool Plantao { get; set; }
            public Complemento Complemento { get; set; }
            public List<DocumentoNecessario> DocumentosNecessarios { get; set; }
        }

        public class Complemento
        {
            public string Tipo { get; set; }
            public string Nome { get; set; }
            public int MaximoDiasAteBloqueio { get; set; }
        }

        public class DocumentoNecessario
        {
            public int Id { get; set; }
            public string NomeDocumento { get; set; }
            public bool Basico { get; set; }
        }

        public class VagasDisponiveisDTO
        {
            public List<Dictionary<string, object>> VagasDisponiveis { get; set; }
        }
}
