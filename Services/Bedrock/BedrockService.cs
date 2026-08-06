using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GatewayConsultaApiVerde.Services.Bedrock
{
    public class BedrockService : IBedrockService
    {
        private readonly IAmazonBedrockRuntime _client;

        private readonly BedrockSettings _settings;


        public BedrockService(
            IAmazonBedrockRuntime client,
            IOptions<BedrockSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }


        public async Task<string> GerarRespostaAsync(string prompt)
        {
            var body = new
            {
                anthropic_version = "bedrock-2023-05-31",

                max_tokens = 350,

                temperature = 0.5,

                top_k = 40,

                top_p = 0.8,

                messages = new[]
                {
                new
                {
                    role = "user",
                    content = prompt
                }
            }
            };


            var request = new InvokeModelRequest
            {
                ModelId = _settings.ModelId,

                ContentType = "application/json",

                Accept = "application/json",

                Body = new MemoryStream(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(body)
                    )
                )
            };


            var response =
                await _client.InvokeModelAsync(request);


            using var reader =
                new StreamReader(response.Body);


            var json =
                await reader.ReadToEndAsync();


            using var document =
                JsonDocument.Parse(json);


            return document
                .RootElement
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString()
                ??
                "";
        }
    }
}
