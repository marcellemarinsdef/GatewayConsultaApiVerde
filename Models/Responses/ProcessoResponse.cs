using System;
using System.Collections.Generic;

namespace GatewayConsultaApiVerde.Models.Responses
{
        public class ProcessoResponse
        {
            public Dados dados { get; set; }
        }

        public class Dados
        {
            public int id { get; set; }
            public string origem { get; set; }
            public int instancia { get; set; }
            public string nomeAssunto { get; set; }
            public string nomeOrgaoJulgador { get; set; }
            public List<Movimento> movimentos { get; set; }
        }

        public class Movimento
        {
            public string titulo { get; set; }
            public DateTime data { get; set; }
            public string descricao { get; set; }
            public string traducao { get; set; }
        }
}
