using EntregasCorreio.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntregasCorreio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FreteController : ControllerBase
    {
        private readonly FreteService _freteService;

        public FreteController(FreteService freteService)
        {
            _freteService = freteService;
        }

        [HttpGet("calcularFrete")]
        public async Task<IActionResult> CalcularPrecoEPrazo(string cepOrigem, string cepDestino, double peso, string modalidade)
        {
            try
            {
                var resultado = await _freteService.CalcularPrecoEPrazo(cepOrigem, cepDestino, peso, modalidade);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }

    }
}



