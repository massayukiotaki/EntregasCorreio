using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EntregasCorreio.Resources
{
    public class FreteReader
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FreteReader> _logger;
        private readonly string _fileId = "18O8Kj3CO2hRO-hk9mub2x9MUtHfp9Fxp";

        public FreteReader(HttpClient httpClient, ILogger<FreteReader> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Dictionary<string,string>> ObterModalidades()
        {
            try
            {
                string url = $"https://drive.google.com/uc?export=download&id={_fileId}";
                _logger.LogInformation("Iniciando requisição para obter modalidades de frete. URL: {Url}", url);

                string conteudo = await _httpClient.GetStringAsync(url);
                _logger.LogInformation("Resposta recebida com sucesso. Tamanho do conteúdo: {Length} caracteres", conteudo.Length);

                // Em FreteReader.cs, ajuste o split para remover espaços e linhas vazias:
                var modalidades = conteudo.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(linha => linha.Split('-'))
                                          .ToDictionary(parts => parts[1].Trim(), parts => parts[0].Trim());

                _logger.LogInformation("Modalidades de frete processadas com sucesso. Total de modalidades: {Count}", modalidades.Count);

                return modalidades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter modalidades de frete do Google Drive");
                throw new InvalidOperationException($"Erro ao ler modalidades do Google Drive. Detalhes: {ex.Message}", ex);
            }
        }
    }
}
