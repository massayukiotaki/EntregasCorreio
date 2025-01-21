using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SeuProjeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        // Endpoint principal
        [HttpPost]
        [Route("calcular")]
        public IActionResult CalcularFrete([FromBody] CalculoFreteRequest request)
        {
            if (!ValidarCep(request.CepOrigem) || !ValidarCep(request.CepDestino))
            {
                return BadRequest("CEP inv�lido.");
            }

            // Obt�m URLs para c�lculo de pre�os e prazos
            var urlsFrete = ObterUrlsFrete(request.CepOrigem, request.CepDestino);

            // Cria op��es de envio
            var opcoesEnvio = new List<OpcaoEnvio>
            {
                new OpcaoEnvio
                {
                    Tipo = "PAC",
                    PrazoEstimado = "5 a 10 dias �teis",
                    UrlPreco = urlsFrete["PAC"].Item1,
                    UrlPrazo = urlsFrete["PAC"].Item2
                },
                new OpcaoEnvio
                {
                    Tipo = "SEDEX",
                    PrazoEstimado = "1 a 3 dias �teis",
                    UrlPreco = urlsFrete["SEDEX"].Item1,
                    UrlPrazo = urlsFrete["SEDEX"].Item2
                }
            };

            // Retorna as op��es de envio como resposta
            return Ok(opcoesEnvio);
        }

        // Valida se o CEP � v�lido
        private bool ValidarCep(string cep)
        {
            return !string.IsNullOrEmpty(cep) && cep.Length == 8 && long.TryParse(cep, out _);
        }

        // Gera URLs simuladas para calcular pre�os e prazos
        private Dictionary<string, (string, string)> ObterUrlsFrete(string cepOrigem, string cepDestino)
        {
            // Simula URLs baseadas nos CEPs
            var urls = new Dictionary<string, (string, string)>
            {
                {
                    "PAC",
                    (
                        $"https://api.correios.com.br/preco/pac?cepOrigem={cepOrigem}&cepDestino={cepDestino}",
                        $"https://api.correios.com.br/prazo/pac?cepOrigem={cepOrigem}&cepDestino={cepDestino}"
                    )
                },
                {
                    "SEDEX",
                    (
                        $"https://api.correios.com.br/preco/sedex?cepOrigem={cepOrigem}&cepDestino={cepDestino}",
                        $"https://api.correios.com.br/prazo/sedex?cepOrigem={cepOrigem}&cepDestino={cepDestino}"
                    )
                }
            };

            return urls;
        }
    }

    // Classe para representar a requisi��o do cliente
    public class CalculoFreteRequest
    {
        public string CepOrigem { get; set; }
        public string CepDestino { get; set; }
    }

    // Classe para representar as op��es de envio
    public class OpcaoEnvio
    {
        public string Tipo { get; set; }           // Tipo de envio: PAC ou SEDEX
        public string PrazoEstimado { get; set; } // Prazo estimado em dias �teis
        public string UrlPreco { get; set; }      // URL para c�lculo de pre�o
        public string UrlPrazo { get; set; }      // URL para c�lculo de prazo
    }
}
