using EntregasCorreio.Models;
using System.Net.Http;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class PrazoService
    {
        private readonly HttpClient _httpClient;

        public PrazoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<PrazoFrete?> getPrazo(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            var optionsPrazo = new JsonSerializerOptions
            {
                Converters = { new PrazoFreteConverter() }
            };

            string urlPrazo = $"https://api.correios.com.br/prazo/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

            var prazoResponse = await _httpClient.GetAsync(urlPrazo);
            prazoResponse.EnsureSuccessStatusCode();

            string jsonPrazo = await prazoResponse.Content.ReadAsStringAsync();
            var prazoFrete = JsonSerializer.Deserialize<PrazoFrete>(jsonPrazo, optionsPrazo);

            Console.WriteLine($"Data Máxima: {prazoFrete.DataMaxEntrega}");
            return prazoFrete;
        }

        // Método público para chamada externa
        public async Task<PrazoFrete?> CalcularPrazo(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            return await getPrazo(cepOrigem, cepDestino, peso, coProduto);
        }
    }
}
