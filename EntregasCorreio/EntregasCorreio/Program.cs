using EntregasCorreio.Models;
using EntregasCorreio.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var token = "eyJhbGciOiJSUzI1NiJ9.eyJpYXQiOjE3Mzc2NDE5NzgsImlzcyI6InRva2VuLXNlcnZpY2UiLCJleHAiOjE3Mzc3MjgzNzgsImp0aSI6IjIyY2M5NTAzLWZiMDMtNDkzMC05OGE4LTJlMWZmNTc1NmZlOCIsImFtYmllbnRlIjoiUFJPRFVDQU8iLCJwZmwiOiJQSiIsImlwIjoiMzQuOTYuNTAuMTM4LCAxOTIuMTY4LjEuMTMwIiwiY2F0IjoiQnowIiwiY2FydGFvLXBvc3RhZ2VtIjp7ImNvbnRyYXRvIjoiOTkxMjYxMDA4NyIsIm51bWVybyI6IjAwNzc3NzU1MDMiLCJkciI6NzQsImFwaXMiOlt7ImFwaSI6Mjd9LHsiYXBpIjozNH0seyJhcGkiOjM1fSx7ImFwaSI6MzZ9LHsiYXBpIjozN30seyJhcGkiOjQxfSx7ImFwaSI6NzZ9LHsiYXBpIjo3OH0seyJhcGkiOjgwfSx7ImFwaSI6ODN9LHsiYXBpIjo4N30seyJhcGkiOjkzfSx7ImFwaSI6NTY2fSx7ImFwaSI6NTg2fSx7ImFwaSI6NTg3fSx7ImFwaSI6NjIxfSx7ImFwaSI6NjIzfV19LCJpZCI6IjAxNTEwMzQ1MDAwMTU4IiwiY25waiI6IjAxNTEwMzQ1MDAwMTU4In0.vwlBUerkiJSrfwsBa81qibjKITmukH79TFYMLsK8vYOMrsyxaRmRt4Gxw5u9pdM_OeG1lVofFjFSj15haRIRJFAn_U8ITX73bnzpuOzlIpcBmWqy-55G6IcjFody8wSB1FJmoVQpLyMK3pcZKT9YPre3_g_4MfsQ8_ReTAF46T_nBKzCHhfvirs8Z6CKJb99xnHKLvf0mW2RHGYCRBbD_es9gkrInqT0i8R2SX8Jx382ZptdPtrmA52RWAIuOnf2vY0Z8OHQFO7AbJzUA63cyorPQRIscsGUviG-fFfM7-Rh7HAD7NwUEwT5SVCET_nApESfT2IUha5CKOunbdG1xw";

builder.Services.AddSingleton(token);

builder.Services.AddHttpClient();

builder.Services.AddScoped<FreteService>();
builder.Services.AddScoped<PrecoService>(); 
builder.Services.AddScoped<PrazoService>();
builder.Services.AddScoped<FreteService>();

builder.Services.AddHttpClient<PrazoService>();
builder.Services.AddHttpClient<FreteService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
