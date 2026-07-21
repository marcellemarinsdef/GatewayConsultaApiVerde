using GatewayConsultaApiVerde.Models;

namespace GatewayConsultaApiVerde.Services
{
    public interface IVerdeApiClient
    {
        Task<PessoaDTO.RespostaDTO?> GetPessoaAsync(string cpfPessoa);
    }
}
