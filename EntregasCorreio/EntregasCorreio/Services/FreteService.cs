using EntregasCorreio.Models;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace EntregasCorreio.Services
{
    public class FreteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token; 

        public FreteService(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token; 
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
                    //colocar o input do user
                    throw new ArgumentException("Modalidade de envio inválida");
                }

                string coProduto = modalidades[modalidade];

                string urlPreco = $"https://api.correios.com.br/preco/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";
                string urlPrazo = $"https://api.correios.com.br/prazo/v1/nacional/{coProduto}?cepOrigem={cepOrigem}&cepDestino={cepDestino}&psObjeto={peso}";

                var precoResponse = await _httpClient.GetAsync(urlPreco);
                precoResponse.EnsureSuccessStatusCode(); 
                PrecoFrete precoFrete = JsonSerializer.Deserialize<PrecoFrete>(await precoResponse.Content.ReadAsStringAsync());
                //criar uma classe FretePreco para tipar a resposta (fretepreco precoData = ...)

                var prazoResponse = await _httpClient.GetAsync(urlPrazo);
                prazoResponse.EnsureSuccessStatusCode(); 
                PrazoFrete prazoFrete = JsonSerializer.Deserialize<PrazoFrete>(await prazoResponse.Content.ReadAsStringAsync());
                //mesma coisa do precoData

                var preco = precoFrete.GetProperty("pcFinal"); 
                var prazo = prazoFrete.GetProperty("dataMaxima");  

                var resultado = new
                {
                    Preco = preco,
                    Prazo = prazo
                };

                return resultado;
            }
            catch (Exception ex)
            {
                return new { Erro = ex.Message };
            }
        }
    }
    //log 
    //permitir adicionar um valor em cima do preço
    //permitir adicionar mais dias no prazo

}


