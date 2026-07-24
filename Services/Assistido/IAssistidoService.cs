using GatewayConsultaApiVerde.Models;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Assistido
{
    public interface IAssistidoService
    {
        Task<AssistidoDTO.RespostaDTO?> GetIdAssistidoAsync(string cpfPessoa);
        Task<JsonDocument> GetAssistidoAsync(string cpfPessoa);
        Task<JsonDocument> CriarAssistidoAsync(CadastrarAssistidoRequestDTO dados);
        Task<JsonDocument> AtualizarAssistidoAsync(string cpfPessoa, AtualizarAssistidoRequestDTO dados);
    }
}
