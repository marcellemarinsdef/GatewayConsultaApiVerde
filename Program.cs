using Amazon.Lambda.AspNetCoreServer.Hosting;
using ApiConsultaProcesso;
using ApiConsultaProcesso.Services;
using DotNetEnv;
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
    .AddHttpClient<IProcessoService, ProcessoService>(client =>
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL_VERDE")
     ?? throw new InvalidOperationException(
         "BASE_URL_VERDE não configurada.");

        client.BaseAddress = new Uri(baseUrl);
    })
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 2;
        options.Retry.Delay = TimeSpan.FromSeconds(2);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(15);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(45);
    });

builder.Services.Configure<ConsultaVerdeSettings>(options =>
{
    options.ClientID = Environment.GetEnvironmentVariable("CLIENT_ID");
    options.Token = Environment.GetEnvironmentVariable("TOKEN");
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
}).WithSummary("Verifica a disponibilidade do endpoint de consulta de processos.")
.WithDescription("""
Utilizado para executar o health check da aplicação antes de sua implantação
nos serviços da AWS.
""");

app.Run();
