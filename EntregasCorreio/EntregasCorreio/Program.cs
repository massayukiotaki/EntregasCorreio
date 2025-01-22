using EntregasCorreio.Models;
using EntregasCorreio.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var token = "eyJhbGciOiJSUzI1NiJ9.eyJpYXQiOjE3Mzc0NzA5NjgsImlzcyI6InRva2VuLXNlcnZpY2UiLCJleHAiOjE3Mzc1NTczNjgsImp0aSI6IjlhNTM0ZWU3LTVhMTQtNDMyMC05MjM2LWFmMzhhNmVhMWI2MSIsImFtYmllbnRlIjoiUFJPRFVDQU8iLCJwZmwiOiJQSiIsImlwIjoiMzQuMzQuMjM0LjEwNSwgMTkyLjE2OC4xLjEzMSIsImNhdCI6IkJ6MCIsImNhcnRhby1wb3N0YWdlbSI6eyJjb250cmF0byI6Ijk5MTI2MTAwODciLCJudW1lcm8iOiIwMDc3Nzc1NTAzIiwiZHIiOjc0LCJhcGlzIjpbeyJhcGkiOjI3fSx7ImFwaSI6MzR9LHsiYXBpIjozNX0seyJhcGkiOjM2fSx7ImFwaSI6Mzd9LHsiYXBpIjo0MX0seyJhcGkiOjc2fSx7ImFwaSI6Nzh9LHsiYXBpIjo4MH0seyJhcGkiOjgzfSx7ImFwaSI6ODd9LHsiYXBpIjo5M30seyJhcGkiOjU2Nn0seyJhcGkiOjU4Nn0seyJhcGkiOjU4N30seyJhcGkiOjYyMX0seyJhcGkiOjYyM31dfSwiaWQiOiIwMTUxMDM0NTAwMDE1OCIsImNucGoiOiIwMTUxMDM0NTAwMDE1OCJ9.ZHjvP6BkuONpTCVnVK0pX5A9ajk9x3-rUp08k7Vo9dJ8VhPoOnktyyipb2PnfXVBBYoz72mZPUyJ5E9PkBsG0I6mtE4vKa7Rn5O1f0BCh83eeENlqA2xY0kmRHeR8S4TVd1Hp8DD2KS7oZXKoVXOwuHyRGVlVVqp0XM-lY8RlE9Hgq3QnNHwZ0phbs7EV1DzhRbj6s944Ea3N1lMrhscFERF_Yo5vEs1g2MzwAqyJmysVRDwCqKzLdZfXLyICv7Ecc7-_LEcyv5ZASHtq_DXFZoRNFnGv39qnh60ZCEEEElTP03Yy2d63I9RyD7DDzAtf-VLTX3iXKw27MjuN-7TxA\\";
builder.Services.AddSingleton(token);

builder.Services.AddHttpClient();

builder.Services.AddScoped<FreteService>();

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
