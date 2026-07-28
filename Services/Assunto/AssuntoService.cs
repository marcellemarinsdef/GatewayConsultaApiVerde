using GatewayConsultaApiVerde.Models;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Assunto
{
    public class AssuntoService : IAssuntoService
    {
        private readonly IConsultaVerdeClient _consultaVerdeClient;

        public AssuntoService(IConsultaVerdeClient consultaVerdeClient)
        {
            _consultaVerdeClient = consultaVerdeClient;
        }

        public Task<JsonDocument> GetCategoriasAsync() =>
            _consultaVerdeClient.GetAsync("assunto/categorias");

        public Task<JsonDocument> ConsultarItemArvoreAsync(int? idCategoria, int? idItemCategoria)
        {
            var query = idItemCategoria.HasValue
                ? $"idItemCategoria={idItemCategoria}"
                : $"idCategoria={idCategoria}";
            return _consultaVerdeClient.GetAsync($"assunto/consultar-item-arvore?{query}");
        }

        public Task<(int StatusCode, JsonDocument? Body)> SugerirMotivoAsync(RelatoRequestDTO dados) =>
            _consultaVerdeClient.PostAsync("assunto/sugerir-motivo", dados);

        public Task<JsonDocument> GetAssuntoAsync(int idAssunto) =>
            _consultaVerdeClient.GetAsync($"assunto/{idAssunto}");
    }
}
