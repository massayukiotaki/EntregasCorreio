using EntregasCorreio.Models;
using System.Net;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class PrecoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public PrecoService(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
        }

        public async Task<PrecoFrete?> ObterPreco(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            string urlPreco = $"https://api.correios.com.br/preco/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

            Console.WriteLine($"Solicitando URL: {urlPreco}");
            Console.WriteLine($"Token: Bearer {_token}");

            var response = await _httpClient.GetAsync(urlPreco);

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException("Acesso negado: verifique o token ou permissões.");

            response.EnsureSuccessStatusCode();

            string jsonPreco = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                Converters = { new PrecoFreteConverter() }
            };

            var precoFrete = JsonSerializer.Deserialize<PrecoFrete>(jsonPreco, options);
            return precoFrete;
        }

    }
}
