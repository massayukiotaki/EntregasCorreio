using EntregasCorreio.Models;
using EntregasCorreio.Resources;
using EntregasCorreio.Services;
using System.Linq;

namespace EntregasCorreio.Services

{
    public class FreteService
    {
        private readonly PrecoService _precoService;
        private readonly PrazoService _prazoService;
        private readonly FreteReader _freteReader;

        public FreteService(PrecoService precoService, PrazoService prazoService, FreteReader freteReader)
        {
            _precoService = precoService;
            _prazoService = prazoService;
            _freteReader = freteReader;
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

            var modalidades = await _freteReader.ObterModalidades();

            if (!modalidades.TryGetValue(modalidade, out string modalidadeCodigo)) 
            { 
                throw new ArgumentException($"Modalidade '{modalidade}' inválida. Opções válidas: {string.Join(", ", modalidades.Keys)}"); 
            }

            string coProduto = modalidadeCodigo;

            Console.WriteLine($"Código da modalidade enviado para API: {coProduto}");

            PrecoFrete? precoFrete = await _precoService.ObterPreco(cepOrigem, cepDestino, peso, coProduto);
            PrazoFrete? prazoFrete = await _prazoService.ObterPrazo(cepOrigem, cepDestino, peso, coProduto);

            return new CorreiosRateResponse(precoFrete.PcFinal, prazoFrete.DataMaxEntrega);
        }
    }
}
