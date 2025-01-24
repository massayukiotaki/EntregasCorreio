using EntregasCorreio.Models;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class FreteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly PrazoService _prazoService;
        private readonly PrecoService _precoService;

        public FreteService(HttpClient httpClient, string token, PrazoService prazoService, PrecoService precoService)
        {
            _httpClient = httpClient;
            _token = token;
            _prazoService = prazoService;
            _precoService = precoService;
        }

        public Dictionary<string, string> ObterModalidades()
        {
            var modalidades = new Dictionary<string, string>
        {
            { "PAC", "03298" },
            { "SEDEX", "03140" },
        };
            return modalidades;
        }

        public async Task<object> CalcularPrecoEPrazo(string cepOrigem, string cepDestino, double peso, string modalidade)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

                var modalidades = ObterModalidades();

                if (!modalidades.ContainsKey(modalidade))
                {
                    throw new ArgumentException("Modalidade de envio inválida");
                }

                string coProduto = modalidades[modalidade];

                PrecoFrete? precoFrete = await _precoService.CalcularPreco(cepOrigem, cepDestino, peso, coProduto);
                PrazoFrete? prazoFrete = await _prazoService.CalcularPrazo(cepOrigem, cepDestino, peso, coProduto);

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

        
    }
}
