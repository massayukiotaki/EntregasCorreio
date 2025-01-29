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

        public async Task<List<string>> ObterModalidades()
        {
            try
            {
                string url = $"https://drive.google.com/uc?export=download&id={_fileId}";
                string conteudo = await _httpClient.GetStringAsync(url);

                // Converte o conteúdo em uma lista de strings (considerando que cada modalidade está em uma linha)
                var modalidades = new List<string>(conteudo.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

                return modalidades;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao acessar arquivo do Google Drive: " + ex.Message);
            }
        }
    }
}
