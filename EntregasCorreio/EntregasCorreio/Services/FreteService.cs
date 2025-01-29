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
            // Aguarda a conclusão da tarefa e obtém as modalidades
            var modalidades = await _freteReader.ObterModalidades();

            // Verifica se a modalidade fornecida está presente na lista de modalidades
            if (!modalidades.Contains(modalidade, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Modalidade de envio inválida.");
            }

            string coProduto = modalidade; // Ajuste conforme necessário

            // Obtém o preço e o prazo do frete
            PrecoFrete? precoFrete = await _precoService.ObterPreco(cepOrigem, cepDestino, peso, coProduto);
            PrazoFrete? prazoFrete = await _prazoService.ObterPrazo(cepOrigem, cepDestino, peso, coProduto);

            // Retorna a resposta formatada
            return new CorreiosRateResponse(precoFrete.PcFinal, prazoFrete.DataMaxEntrega);
        }
    }
}
