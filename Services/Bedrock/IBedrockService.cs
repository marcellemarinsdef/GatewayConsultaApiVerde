namespace GatewayConsultaApiVerde.Services.Bedrock
{
    public interface IBedrockService
    {
        Task<string> GerarRespostaAsync(string prompt);
    }
}
