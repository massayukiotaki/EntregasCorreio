using EntregasCorreio.Models;
using System.Net;
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

        public async Task<PrecoFrete?> ObterPreco(string cepOrigem, string cepDestino, double peso, string coProduto)
        {
            DotNetEnv.Env.Load("./Environments/.env");
            string token = DotNetEnv.Env.GetString("TOKEN");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            string urlPreco = $"https://api.correios.com.br/preco/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

            Console.WriteLine($"Solicitando URL: {urlPreco}");

            var response = await _httpClient.GetAsync(urlPreco);

            //corrigir a ordem do sucess
            //faixa client de 400 a 499
            //faixa server de 500 a 599

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
