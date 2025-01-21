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
            // Valida o token de autentica��o
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader) ||
                !ValidarToken(authorizationHeader))
            {
                return Unauthorized("Token inv�lido ou ausente.");
            }

            // Valida os CEPs
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

        // Valida o token (simula��o)
        private bool ValidarToken(string authorizationHeader)
        {
            // Remove o prefixo "Bearer" se presente
            var token = authorizationHeader.StartsWith("Bearer ")
                ? authorizationHeader.Substring("Bearer ".Length).Trim()
                : authorizationHeader;

            // Simula valida��o do token (substituir pela l�gica real, como consulta em um banco ou verifica��o JWT)
            return token == "eyJhbGciOiJSUzI1NiJ9.eyJpYXQiOjE3MzczODQ5MDEsImlzcyI6InRva2VuLXNlcnZpY2UiLCJleHAiOjE3Mzc0NzEzMDEsImp0aSI6IjBlMmUwNGRlLWEzMWYtNDkxNi1iZTNlLTdlM2EwODE4OTdkOCIsImFtYmllbnRlIjoiUFJPRFVDQU8iLCJwZmwiOiJQSiIsImlwIjoiMzQuMzQuMjM0LjkzLCAxOTIuMTY4LjEuMTMwIiwiY2F0IjoiQnowIiwiY2FydGFvLXBvc3RhZ2VtIjp7ImNvbnRyYXRvIjoiOTkxMjYxMDA4NyIsIm51bWVybyI6IjAwNzc3NzU1MDMiLCJkciI6NzQsImFwaXMiOlt7ImFwaSI6Mjd9LHsiYXBpIjozNH0seyJhcGkiOjM1fSx7ImFwaSI6MzZ9LHsiYXBpIjozN30seyJhcGkiOjQxfSx7ImFwaSI6NzZ9LHsiYXBpIjo3OH0seyJhcGkiOjgwfSx7ImFwaSI6ODN9LHsiYXBpIjo4N30seyJhcGkiOjkzfSx7ImFwaSI6NTY2fSx7ImFwaSI6NTg2fSx7ImFwaSI6NTg3fSx7ImFwaSI6NjIxfSx7ImFwaSI6NjIzfV19LCJpZCI6IjAxNTEwMzQ1MDAwMTU4IiwiY25waiI6IjAxNTEwMzQ1MDAwMTU4In0.ERXQgGhc3tFrPzx62-1qhgGnX2lr3Z8OPXmQGbnfkuiy3VTGic1M7zFJHdQfUr5a1pEK7ouxLpwnzXpcvseCvQNKoKZvjK6Wv16Rffr42an6zy5nLFUXK3pJWtrEPnfC7iuwaLY--Y_3e469syljOdyz0V4UHS984tQTijXQbRty9AH3LxHnergVoTdN9a86BdEbZrTRxOgG9NJlq31KcBHOqGXz4LC3-f0xyyg6NMV9pDV4eKS1s4naCSkKQMsXQ8bIbJwkeIr1MUpOacbABPeOOHWju_1UXMT8SuRIpeVoKAvplhJgIIppfx3Qk_HuwkxxV2twjQiMv2ohlOBQ1Q";
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