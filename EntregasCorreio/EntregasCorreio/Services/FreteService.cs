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
        private readonly ILogger<FreteService> _logger;

        public FreteService(PrecoService precoService, PrazoService prazoService, FreteReader freteReader, ILogger<FreteService> logger)
        {
            _precoService = precoService;
            _prazoService = prazoService;
            _freteReader = freteReader;
            _logger = logger;
        }

        public string FormatarCep(string cep)
        {
            _logger.LogInformation("Formatando CEP: {Cep}", cep);

            if (string.IsNullOrWhiteSpace(cep))
            {
                _logger.LogWarning("CEP recebido é nulo ou vazio.");
                throw new ArgumentException("O CEP não pode ser vazio ou nulo.");
            }

            string cepFormatado = new string(cep.Where(char.IsDigit).ToArray());

            if (cepFormatado.Length != 8)
            {
                _logger.LogWarning("CEP inválido. Deve conter exatamente 8 dígitos. CEP recebido: {CepFormatado}", cepFormatado);
                throw new ArgumentException("O CEP deve conter exatamente 8 dígitos.");
            }

            _logger.LogInformation("CEP formatado com sucesso: {CepFormatado}", cepFormatado);
            return cepFormatado;
        }

        public async Task<object> CalcularPrecoEPrazo(string cepOrigem, string cepDestino, double peso, string modalidade)
        {
            _logger.LogInformation("Iniciando cálculo de preço e prazo. Origem: {CepOrigem}, Destino: {CepDestino}, Peso: {Peso}, Modalidade: {Modalidade}",
                    cepOrigem, cepDestino, peso, modalidade);

            var modalidades = await _freteReader.ObterModalidades();

            if (!modalidades.TryGetValue(modalidade, out string modalidadeCodigo)) 
            {
                _logger.LogWarning("Modalidade '{Modalidade}' inválida. Modalidades disponíveis: {ModalidadesDisponiveis}",
                        modalidade, string.Join(", ", modalidades.Keys));

                throw new ArgumentException($"Modalidade '{modalidade}' inválida. Opções válidas: {string.Join(", ", modalidades.Keys)}"); 
            }

            string coProduto = modalidadeCodigo;
            _logger.LogInformation("Código da modalidade enviado para API: {CoProduto}", coProduto);

            PrecoFrete? precoFrete = await _precoService.ObterPreco(cepOrigem, cepDestino, peso, coProduto);
            if (precoFrete == null)
            {
                _logger.LogError("Falha ao obter o preço do frete. Parâmetros: Origem: {CepOrigem}, Destino: {CepDestino}, Peso: {Peso}, Modalidade: {Modalidade}",
                    cepOrigem, cepDestino, peso, modalidade);
                throw new Exception("Erro ao obter preço do frete.");
            }

            PrazoFrete? prazoFrete = await _prazoService.ObterPrazo(cepOrigem, cepDestino, peso, coProduto);
            if (precoFrete == null)
            {
                _logger.LogError("Falha ao obter o preço do frete. Parâmetros: Origem: {CepOrigem}, Destino: {CepDestino}, Peso: {Peso}, Modalidade: {Modalidade}",
                    cepOrigem, cepDestino, peso, modalidade);
                throw new Exception("Erro ao obter preço do frete.");
            }

            _logger.LogInformation("Cálculo de frete concluído com sucesso. Preço: {Preco}, Prazo: {DataMaxEntrega}",
                    precoFrete.PcFinal, prazoFrete.DataMaxEntrega);

            return new CorreiosRateResponse(precoFrete.PcFinal, prazoFrete.DataMaxEntrega);
        }
    }
}
