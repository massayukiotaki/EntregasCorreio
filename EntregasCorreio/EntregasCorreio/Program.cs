using System.Reflection;
using EntregasCorreio.Controllers;
using EntregasCorreio.Services;
using Microsoft.OpenApi.Models;
using EntregasCorreio.Resources;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
