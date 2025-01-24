using EntregasCorreio.Models;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class FreteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly PrazoService _prazoService; // Dependência para PrazoService

        public FreteService(HttpClient httpClient, string token, PrazoService prazoService)
        {
            _httpClient = httpClient;
            _token = token;
            _prazoService = prazoService; // Inicializando PrazoService
        }

        public string FormatarCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                throw new ArgumentException("O CEP não pode ser vazio ou nulo.");

            string cepFormatado = new string(cep.Where(char.IsDigit).ToArray());

            if (cepFormatado.Length != 8)
                throw new ArgumentException("O CEP deve conter exatamente 8 dígitos.");

            return cepFormatado;
        }

        public async Task<object> CalcularPrecoEPrazo(string cepOrigem, string cepDestino, double peso, string modalidade)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

                var modalidades = new Dictionary<string, string>
                {
                    { "PAC", "03298" },
                    { "SEDEX", "03140" },
                };

                if (!modalidades.ContainsKey(modalidade))
                {
                    throw new ArgumentException("Modalidade de envio inválida");
                }

                string coProduto = modalidades[modalidade];

                PrecoFrete? precoFrete = await getPreco(cepOrigem, cepDestino, peso, coProduto);
                PrazoFrete? prazoFrete = await _prazoService.CalcularPrazo(cepOrigem, cepDestino, peso, coProduto); // Chama o método da PrazoService

                // Criando o resultado com os valores corretos
                var resultado = new
                {
                    Preco = precoFrete?.PcFinal,
                    Prazo = prazoFrete?.DataMaxEntrega
                };

                return resultado;
            }
            catch (Exception ex)
            {
                return new { Erro = ex.Message };
            }
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
    }
}
