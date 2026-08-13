using Amazon;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using DotNetEnv;
using GatewayConsultaApiVerde;
using GatewayConsultaApiVerde.Services.Agendamento;
using GatewayConsultaApiVerde.Services.Agendamentos;
using GatewayConsultaApiVerde.Services.Assistido;
using GatewayConsultaApiVerde.Services.Assunto;
using GatewayConsultaApiVerde.Services.Bloqueio;
using GatewayConsultaApiVerde.Services.Casos;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using GatewayConsultaApiVerde.Services.Encaminhamento;
using GatewayConsultaApiVerde.Services.Orgao;
using GatewayConsultaApiVerde.Services.Plantao;
using GatewayConsultaApiVerde.Services.Processo;
using GatewayConsultaApiVerde.Services.Recesso;
using System.Reflection;
using System.Text.Json;

if (File.Exists(".env"))
{
    Env.Load();
}
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuração do HttpClient

builder.Services
    .AddHttpClient<IConsultaVerdeClient, ConsultaVerdeClient>(client =>
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL_VERDE")
     ?? throw new InvalidOperationException(
         "BASE_URL_VERDE não configurada.");

        client.BaseAddress = new Uri(baseUrl);
    }).AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 2;
        options.Retry.Delay = TimeSpan.FromSeconds(2);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(45);
    });

builder.Services.AddScoped<IAssistidoService, AssistidoService>();
builder.Services.AddScoped<ICasosService, CasosService>();
builder.Services.AddScoped<IAgendamentosService, AgendamentosService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();
builder.Services.AddScoped<IProcessoService, ProcessoService>();
builder.Services.AddScoped<IOrgaoService, OrgaoService>();
builder.Services.AddScoped<IAssuntoService, AssuntoService>();
builder.Services.AddScoped<IEncaminhamentoService, EncaminhamentoService>();
builder.Services.AddScoped<IBloqueioService, BloqueioService>();
builder.Services.AddScoped<IPlantaoService, PlantaoService>();
builder.Services.AddScoped<IRecessoService, RecessoService>();

// Credenciais do Verde (CLIENT_ID/TOKEN) vêm do Secrets Manager em produção —
// TOKEN é um JWT que expira periodicamente, então não pode viver só em env
// var plana do Lambda (já causou 401 em produção por token vencido, sem
// nenhum jeito de rotacionar sem redeploy). Fallback pra env var cobre
// ambiente local sem credencial/permissão AWS pra Secrets Manager.
using var bootstrapLoggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var bootstrapLogger = bootstrapLoggerFactory.CreateLogger("Startup.CredenciaisVerde");

var secretsManagerClient = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);
builder.Services.AddSingleton<IAmazonSecretsManager>(secretsManagerClient);

// Carregado sob demanda (Lazy), NÃO no boot do processo: o container e o
// endpoint /health não podem passar a depender de credencial do Verde
// configurada (quebraria o smoke test do CI, que sobe a imagem sem
// nenhuma env var de Verde só pra checar que o app inicia). A busca real
// só acontece quando algum serviço que precisa do Verde é resolvido pela
// primeira vez (IOptions<ConsultaVerdeSettings> é lazy por padrão), e uma
// única vez graças ao Lazy<Task<>>.
var credenciaisVerdeLazy = new Lazy<Task<(string ClientId, string Token)>>(
    () => CarregarCredenciaisVerdeAsync(secretsManagerClient, bootstrapLogger),
    LazyThreadSafetyMode.ExecutionAndPublication);

builder.Services.Configure<ConsultaVerdeSettings>(options =>
{
    var credenciais = credenciaisVerdeLazy.Value.GetAwaiter().GetResult();
    options.ClientID = credenciais.ClientId;
    options.Token = credenciais.Token;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NextJs", policy =>
    {
        policy.WithOrigins(
     "http://localhost:3000",
     "https://localhost:3000"
 );
    });
});
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("NextJs");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        service = "ApiConsultaProcesso",
        status = "healthy"
    });
})
.WithTags("HealthCheck")
.WithSummary("Verifica a disponibilidade do endpoint de consulta de processos.")
.WithDescription("""
Utilizado para executar o health check da aplicação antes de sua implantação
nos serviços da AWS.
""");

app.Run();

// Busca CLIENT_ID/TOKEN do secret "maria-chat-prod/gateway-verde" no Secrets
// Manager (JSON { "CLIENT_ID": "...", "TOKEN": "..." }). Se a busca falhar
// (sem credencial/permissão AWS — caso comum em desenvolvimento local), cai
// pro fallback de env var pra não quebrar o startup. Nunca loga o valor das
// credenciais, só qual fonte foi usada.
static async Task<(string ClientId, string Token)> CarregarCredenciaisVerdeAsync(
    IAmazonSecretsManager secretsManager,
    ILogger logger)
{
    const string secretId = "maria-chat-prod/gateway-verde";

    try
    {
        var response = await secretsManager.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = secretId
        });

        using var document = JsonDocument.Parse(response.SecretString);
        var root = document.RootElement;

        var clientId = root.GetProperty("CLIENT_ID").GetString();
        var token = root.GetProperty("TOKEN").GetString();

        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException(
                $"Secret '{secretId}' não contém CLIENT_ID/TOKEN válidos.");

        logger.LogInformation(
            "Credenciais do Verde carregadas via Secrets Manager ({SecretId}).",
            secretId);

        return (clientId, token);
    }
    catch (Exception ex)
    {
        logger.LogWarning(
            ex,
            "Não foi possível carregar credenciais do Verde via Secrets Manager " +
            "({SecretId}); usando fallback de env var (CLIENT_ID/TOKEN).",
            secretId);

        var clientId = Environment.GetEnvironmentVariable("CLIENT_ID")
            ?? throw new InvalidOperationException(
                "CLIENT_ID não foi definida (falhou no Secrets Manager e não há env var).");

        var token = Environment.GetEnvironmentVariable("TOKEN")
            ?? throw new InvalidOperationException(
                "TOKEN não foi definida (falhou no Secrets Manager e não há env var).");

        return (clientId, token);
    }
}
