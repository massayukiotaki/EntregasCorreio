using EntregasCorreio.Models;
using System.Net.Http;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class PrecoService
    {
        private readonly HttpClient _httpClient;

        public PrecoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private async Task<PrecoFrete?> getPreco(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            string urlPreco = $"https://api.correios.com.br/preco/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

            var optionsPreco = new JsonSerializerOptions
            {
                Converters = { new PrecoFreteConverter() }
            };

            var precoResponse = await _httpClient.GetAsync(urlPreco);
            precoResponse.EnsureSuccessStatusCode();

            string jsonPreco = await precoResponse.Content.ReadAsStringAsync();
            var precoFrete = JsonSerializer.Deserialize<PrecoFrete>(jsonPreco, optionsPreco);

            Console.WriteLine($"Preço Final: {precoFrete?.PcFinal}");
            return precoFrete;
        }

        public async Task<PrecoFrete?> CalcularPreco(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            return await getPreco(cepOrigem, cepDestino, peso, coProduto);
        }

    }
}
