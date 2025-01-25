using System.Reflection;
using EntregasCorreio.Controllers;
using EntregasCorreio.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var token = "eyJhbGciOiJSUzI1NiJ9.eyJpYXQiOjE3Mzc3Mjc5MTQsImlzcyI6InRva2VuLXNlcnZpY2UiLCJleHAiOjE3Mzc4MTQzMTQsImp0aSI6IjY4NTRkNTM3LTlmOTYtNDJiYi1hMjVmLWE2ODMzMmRiNzI3ZSIsImFtYmllbnRlIjoiUFJPRFVDQU8iLCJwZmwiOiJQSiIsImlwIjoiMzQuOTYuNTAuNTksIDE5Mi4xNjguMS4xMzAiLCJjYXQiOiJCejAiLCJjYXJ0YW8tcG9zdGFnZW0iOnsiY29udHJhdG8iOiI5OTEyNjEwMDg3IiwibnVtZXJvIjoiMDA3Nzc3NTUwMyIsImRyIjo3NCwiYXBpcyI6W3siYXBpIjoyN30seyJhcGkiOjM0fSx7ImFwaSI6MzV9LHsiYXBpIjozNn0seyJhcGkiOjM3fSx7ImFwaSI6NDF9LHsiYXBpIjo3Nn0seyJhcGkiOjc4fSx7ImFwaSI6ODB9LHsiYXBpIjo4M30seyJhcGkiOjg3fSx7ImFwaSI6OTN9LHsiYXBpIjo1NjZ9LHsiYXBpIjo1ODZ9LHsiYXBpIjo1ODd9LHsiYXBpIjo2MjF9LHsiYXBpIjo2MjN9XX0sImlkIjoiMDE1MTAzNDUwMDAxNTgiLCJjbnBqIjoiMDE1MTAzNDUwMDAxNTgifQ.fDN7-qg3jXXCLxytsyF4S7Mf3WbF7lK2yJWkupU9EsJDusdSNQzG2AD8SLZE61KYLVNJH_Tu14GYSP-hBegY1MeHy9IMzLunGVvgzkOKUvpalF6slJrB-yi3aOACgyCv0CVoxAJylsdqaCaYP2JDktFHemjWy1UimYB11QSfiaEVw-mvheqfZeCK10Vq7AVlBBqdEYYPw21k_6WCsMTUpbIbN2LktYUQrhjDtQZlBdy678shJ0l4LR4E7bn9I0EWPaDaRi_sSVm_cjQv4q3Pz-hkyum9B_9AZoJWSoHxdzxyd85c_Wf3k430l4Ue0x8k3hQogc37gRVqORBcHtUBfA";

builder.Services.AddSingleton(token);

builder.Services.AddHttpClient();

builder.Services.AddScoped<PrecoService>();
builder.Services.AddScoped<PrazoService>();
builder.Services.AddScoped<FreteService>();


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
