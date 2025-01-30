using EntregasCorreio.Models;
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

        public async Task<PrazoFrete?> ObterPrazo(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            DotNetEnv.Env.Load("./Environments/.env");
            string token = DotNetEnv.Env.GetString("TOKEN");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

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
