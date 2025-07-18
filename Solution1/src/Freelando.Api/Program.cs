using Freelando.Api.Converters;
using Freelando.Api.Endpoints;
using Freelando.Dados;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FreelandoContext>((options) =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});
builder.Services.AddScoped<ProjetoConverter>();
builder.Services.AddScoped<ClienteConverter>();
builder.Services.AddScoped<ContratoConverter>();
builder.Services.AddScoped<ServicoConverter>();
builder.Services.AddScoped<CandidaturaConverter>();
builder.Services.AddScoped<ProfissionalConverter>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddEndPointEspecialidades();
app.AddEndPointProfissional();
app.AddEndPointProjeto();
app.AddEndPointContrato();
app.AddEndPointCandidatura();
app.AddEndPointServico();
app.AddEndPointClientes();

app.UseHttpsRedirection();

app.Run();
