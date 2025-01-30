using EntregasCorreio.Models;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging; // Adicione isso para usar ILogger

namespace EntregasCorreio.Services
{
    public class PrecoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly ILogger<PrecoService> _logger;

        public PrecoService(HttpClient httpClient, string token, ILogger<PrecoService> logger)
        {
            _httpClient = httpClient;
            _token = token;
            _logger = logger;
        }

        public async Task<PrecoFrete?> ObterPreco(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            try
            {
                _logger.LogTrace("Iniciando a solicitação para obter o preço do frete.");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

                string urlPreco = $"https://api.correios.com.br/preco/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

                _logger.LogDebug("URL construída: {Url}", urlPreco);
                _logger.LogDebug("Token usado: Bearer {Token}", _token);

                var response = await _httpClient.GetAsync(urlPreco);

                if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
                {
                    _logger.LogWarning("Erro na solicitação. Código: {StatusCode}. Verifique os dados enviados.", response.StatusCode);
                    throw new HttpRequestException($"Erro cliente: {response.StatusCode}");
                }
                else if ((int)response.StatusCode >= 500 && (int)response.StatusCode <= 599)
                {
                    _logger.LogError("Erro no servidor dos Correios. Por favor tente mais tarde Código: {StatusCode}", response.StatusCode);
                    throw new HttpRequestException($"Erro servidor: {response.StatusCode}");
                }

                response.EnsureSuccessStatusCode();

                string jsonPreco = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Resposta recebida com sucesso. Processando os dados.");

                var options = new JsonSerializerOptions
                {
                    Converters = { new PrecoFreteConverter() }
                };

                var precoFrete = JsonSerializer.Deserialize<PrecoFrete>(jsonPreco, options);
                _logger.LogTrace("Preço do frete desserializado com sucesso.");
                return precoFrete;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Acesso negado ao obter preço do frete. Verifique o token ou as permissões.");
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro HTTP ao acessar a API dos Correios.");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Erro ao desserializar a resposta da API dos Correios.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Erro inesperado ao obter o preço do frete.");
                throw;
            }
        }
    }
}
