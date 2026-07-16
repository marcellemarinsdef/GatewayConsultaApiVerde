using ApiConsultaProcesso;
using ApiConsultaProcesso.Services;
using DotNetEnv;

if (File.Exists(".env"))
{
    Env.Load();
}
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuração do HttpClient

builder.Services
    .AddHttpClient<IProcessoService, ProcessoService>(client =>
    {
        client.BaseAddress = new Uri(
            Environment.GetEnvironmentVariable("BASE_URL_VERDE")!);
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NextJs");

app.UseAuthorization();

app.MapControllers();

app.Run();
