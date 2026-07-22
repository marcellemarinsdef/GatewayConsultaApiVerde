using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services
{
    public interface IAssistidoService
    {
        Task<AssistidoDTO.RespostaDTO?> GetAssistidoAsync(string cpfPessoa);
    }
}
