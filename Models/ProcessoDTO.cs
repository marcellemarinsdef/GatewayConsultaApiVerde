namespace ApiConsultaProcesso.Models;
using System.Text.Json.Serialization;


    public class ProcessoDTO
    {
        public class RespostaDTO
        {
        [JsonPropertyName("dados")]
        public Processo Dados { get; set; }
        }
        public class Processo
        {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("origem")]
        public string Origem { get; set; }
        [JsonPropertyName("instancia")]
        public int Instancia { get; set; }
        [JsonPropertyName("nomeAssunto")]
        public string NomeAssunto { get; set; }
        [JsonPropertyName("nomeOrgaoJulgador")]
        public string NomeOrgaoJulgador { get; set; }
        [JsonPropertyName("movimentos")]
        public List<MovimentosDTO> Movimentos { get; set; } = new List<MovimentosDTO>();
        }
    }

