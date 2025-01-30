using System.Reflection;
using EntregasCorreio.Controllers;
using EntregasCorreio.Services;
using Microsoft.OpenApi.Models;
using EntregasCorreio.Resources;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var token = "eyJhbGciOiJSUzI1NiJ9.eyJpYXQiOjE3MzgyNDE5OTIsImlzcyI6InRva2VuLXNlcnZpY2UiLCJleHAiOjE3MzgzMjgzOTIsImp0aSI6ImJlNWVjMGQ0LTlkNzktNDI1OC05YzEwLTlmMWQ4YWVlNzVjNiIsImFtYmllbnRlIjoiUFJPRFVDQU8iLCJwZmwiOiJQSiIsImlwIjoiMzQuMzQuMjM0LjEwMCwgMTkyLjE2OC4xLjEzMCIsImNhdCI6IkJ6MCIsImNhcnRhby1wb3N0YWdlbSI6eyJjb250cmF0byI6Ijk5MTI2MTAwODciLCJudW1lcm8iOiIwMDc3Nzc1NTAzIiwiZHIiOjc0LCJhcGlzIjpbeyJhcGkiOjI3fSx7ImFwaSI6MzR9LHsiYXBpIjozNX0seyJhcGkiOjM2fSx7ImFwaSI6Mzd9LHsiYXBpIjo0MX0seyJhcGkiOjc2fSx7ImFwaSI6Nzh9LHsiYXBpIjo4MH0seyJhcGkiOjgzfSx7ImFwaSI6ODd9LHsiYXBpIjo5M30seyJhcGkiOjU2Nn0seyJhcGkiOjU4Nn0seyJhcGkiOjU4N30seyJhcGkiOjYyMX0seyJhcGkiOjYyM31dfSwiaWQiOiIwMTUxMDM0NTAwMDE1OCIsImNucGoiOiIwMTUxMDM0NTAwMDE1OCJ9.MBGLVEtynO-XFg9FiYKj669cJM_55CMY9FCwm2h9ucey8HT_plHq6p80KeuQ1VzHaUQLueODV5XWNkt55Z3r6Fjs13RnT56WUi0xnNnQM-Dw_zzBtnvP0fJG_yth7XZm3XpPw-fktTlnqNtVQuFj_sK1TfPrh67NW-ph1RhRFEY19X0ynBuugCzKqJW9tFfQDcfSZ4zKqTbjV7oeHWJOcG71ZrZJyRwUeLYrRkdPfyw3LjBPRtobgZixJV9HiyVldTKbDWlRCP_oUny8lZhwPeuMJu6Zi6YRA4xVFmlFFxsBedAEh2A6ODAOBAYVR0qzrur987i0GXOWew0bXd3gcg";

builder.Services.AddSingleton(token);

builder.Services.AddHttpClient();

builder.Services.AddScoped<PrecoService>();
builder.Services.AddScoped<PrazoService>();
builder.Services.AddScoped<FreteService>();
builder.Services.AddScoped<FreteReader>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Entregas Correios",
        Description = "Obtem preço e prazo de entregas via Correios de um objeto levando em conta seu peso, CEP de origem e CEP de destino utilizando uma modalidade de serviço especificada.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Contato",
            Url = new Uri("https://itlab.com.br/pages/pt/contato/")
        },
        License = new OpenApiLicense
        {
            Name = "Todos os direitos reservados",
            Url = new Uri("https://itlab.com.br")
        }
    }
    );

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.ExampleFilters();
    options.EnableAnnotations();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<FreteController>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
