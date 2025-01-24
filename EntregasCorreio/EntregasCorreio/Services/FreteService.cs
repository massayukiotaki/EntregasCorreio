using EntregasCorreio.Models;

namespace EntregasCorreio.Services

{
    public class FreteService
    {
        private readonly PrecoService _precoService;
        private readonly PrazoService _prazoService;

        public FreteService(PrecoService precoService, PrazoService prazoService)
        {
            _precoService = precoService;
            _prazoService = prazoService;
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
            var modalidades = new Dictionary<string, string>
            {
                { "PAC", "03298" },
                { "SEDEX", "03140" },
            };

            if (!modalidades.ContainsKey(modalidade))
                throw new ArgumentException("Modalidade de envio inválida");

            string coProduto = modalidades[modalidade];

            PrecoFrete? precoFrete = await _precoService.ObterPreco(cepOrigem, cepDestino, peso, coProduto);
            PrazoFrete? prazoFrete = await _prazoService.ObterPrazo(cepOrigem, cepDestino, peso, coProduto);

            return new
            {
                Preco = precoFrete.PcFinal,
                Prazo = prazoFrete.DataMaxEntrega
            };
        }
    }
}
