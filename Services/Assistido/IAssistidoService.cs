using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services.Assistido
{
    public interface IAssistidoService
    {
        Task<AssistidoDTO.RespostaDTO?> GetAssistidoAsync(string cpfPessoa);
    }
}
