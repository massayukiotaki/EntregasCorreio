using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EntregasCorreio.Resources
{
    public class FreteReader
    {
        private readonly HttpClient _httpClient;
        private readonly string _fileId = "18O8Kj3CO2hRO-hk9mub2x9MUtHfp9Fxp";

        public FreteReader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string,string>> ObterModalidades()
        {
            try
            {
                string url = $"https://drive.google.com/uc?export=download&id={_fileId}";
                string conteudo = await _httpClient.GetStringAsync(url);

                // Em FreteReader.cs, ajuste o split para remover espaços e linhas vazias:
                var modalidades = conteudo.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(linha => linha.Split('-'))
                                          .ToDictionary(parts => parts[1].Trim(), parts => parts[0].Trim());

                return modalidades;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao ler modalidades do Google Drive. Detalhes: {ex.Message}", ex);
            }
        }
    }
}
