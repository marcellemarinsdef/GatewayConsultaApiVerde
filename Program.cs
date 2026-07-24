using Amazon.Lambda.AspNetCoreServer.Hosting;
using DotNetEnv;
using GatewayConsultaApiVerde;
using GatewayConsultaApiVerde.Services.Agendamento;
using GatewayConsultaApiVerde.Services.Agendamentos;
using GatewayConsultaApiVerde.Services.Assistido;
using GatewayConsultaApiVerde.Services.Casos;
using GatewayConsultaApiVerde.Services.ConsultasBase;
using GatewayConsultaApiVerde.Services.Processo;
using System.Reflection;

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


builder.Services.Configure<ConsultaVerdeSettings>(options =>
{
    options.ClientID = Environment.GetEnvironmentVariable("CLIENT_ID")
       ?? throw new InvalidOperationException("CLIENT_ID não foi definida.");

    options.Token = Environment.GetEnvironmentVariable("TOKEN")
        ?? throw new InvalidOperationException("TOKEN não foi definida.");
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
