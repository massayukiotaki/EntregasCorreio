using EntregasCorreio.Models;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class PrazoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public PrazoService(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
        }

        public async Task<PrazoFrete?> ObterPrazo(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            string urlPrazo = $"https://api.correios.com.br/prazo/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

            var optionsPrazo = new JsonSerializerOptions
            {
                Converters = { new PrazoFreteConverter() }
            };

            var prazoResponse = await _httpClient.GetAsync(urlPrazo);
            prazoResponse.EnsureSuccessStatusCode();

            string jsonPrazo = await prazoResponse.Content.ReadAsStringAsync();
            var prazoFrete = JsonSerializer.Deserialize<PrazoFrete>(jsonPrazo, optionsPrazo);

            Console.WriteLine($"Data Máxima: {prazoFrete.DataMaxEntrega}");
            return prazoFrete;
        }
    }
}
