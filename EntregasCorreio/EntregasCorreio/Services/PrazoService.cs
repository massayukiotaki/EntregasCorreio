using EntregasCorreio.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class PrazoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public PrazoService(HttpClient httpClient, ILogger<PrazoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PrazoFrete?> ObterPrazo(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            try
            {
                DotNetEnv.Env.Load("./Environments/.env");
                string token = DotNetEnv.Env.GetString("TOKEN");

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("O token da API dos Correios não foi encontrado.");
                    throw new InvalidOperationException("Token da API dos Correios não está configurado.");
                }

                _logger.LogInformation("Token carregado com sucesso.");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                string urlPrazo = $"https://api.correios.com.br/prazo/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

                _logger.LogInformation("Enviando requisição para a API dos Correios: {Url}", urlPrazo);

                var optionsPrazo = new JsonSerializerOptions
                {
                    Converters = { new PrazoFreteConverter() }
                };

                var prazoResponse = await _httpClient.GetAsync(urlPrazo);

                if (!prazoResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Falha ao obter prazo. Status: {StatusCode}, Motivo: {ReasonPhrase}",
                        (int)prazoResponse.StatusCode, prazoResponse.ReasonPhrase);
                    return null;
                }

                string jsonPrazo = await prazoResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("Resposta recebida com sucesso. Conteúdo: {Json}", jsonPrazo);

                var prazoFrete = JsonSerializer.Deserialize<PrazoFrete>(jsonPrazo, optionsPrazo);

                _logger.LogInformation("Data Máxima de Entrega: {DataMax}", prazoFrete?.DataMaxEntrega);

                return prazoFrete;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Erro ao obter prazo de entrega.");
                throw;
            }
        }
    }
}
