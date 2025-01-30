using EntregasCorreio.Resources;
using EntregasCorreio.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EntregasCorreio.Controllers
{
    [ApiController]
    [Route("api/correios/[controller]")]
    [Produces("application/json")]
    public class FreteController : ControllerBase
    {
        private readonly FreteService _freteService;
        private readonly ILogger<FreteReader> _logger;

        public FreteController(FreteService freteService, ILogger<FreteReader> logger)
        {
            _freteService = freteService;
            _logger = logger;
        }

        /// <param name="cepOrigem">CEP de origem. Exemplo: 28625720</param>
        /// <param name="cepDestino">CEP de destino. Exemplo: 05271000</param>
        /// <param name="peso">Peso do pacote em gramas. Exemplo: 450</param>
        /// <param name="modalidade">Modalidade de entrega (ex.: PAC, SEDEX). Exemplo: PAC</param>
        /// <returns>JSON contendo preço e prazo</returns>
        /// <response code="400">Se os parâmetros forem inválidos</response>
        /// <response code="403">Se token inválido ou sem permissões para a API dos Correios</response>
        [SwaggerOperation(
            Summary = "Calcula o preço e prazo do frete",
            Description = "Obtem o preço e prazo com base nos parâmetros fornecidos. [Mais detalhes aqui](https://www.correios.com.br/)."
        )]
        [SwaggerResponse(200, "Preço e prazo retornados com sucesso", typeof(object))]
        [HttpGet("calcularFrete")]
        public async Task<IActionResult> CalcularPrecoEPrazo(
                [FromQuery] string cepOrigem = "28625720",
                [FromQuery] string cepDestino = "05271000",
                [FromQuery] double peso = 450,
                [FromQuery] string modalidade = "PAC")
        {
            _logger.LogInformation("Recebida solicitação para cálculo de frete. Origem: {CepOrigem}, Destino: {CepDestino}, Peso: {Peso}, Modalidade: {Modalidade}",
                    cepOrigem, cepDestino, peso, modalidade);

            try
            {
                _logger.LogInformation("Chamando FreteService para calcular preço e prazo...");
                var resultado = await _freteService.CalcularPrecoEPrazo(cepOrigem, cepDestino, peso, modalidade);

                _logger.LogInformation("Cálculo de frete concluído com sucesso. Resposta: {@Resultado}", resultado);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular frete. Origem: {CepOrigem}, Destino: {CepDestino}, Peso: {Peso}, Modalidade: {Modalidade}",
                        cepOrigem, cepDestino, peso, modalidade);
                return BadRequest($"Erro ao calcular frete: {ex.GetType()} - {ex.Message} - {ex.StackTrace}");
            }
        }


    }

}



