namespace EntregasCorreio.Models
{
    public class FreteRequest
    {
        public string CepOrigem { get; set; }
        public string CepDestino { get; set; }
        public double Peso { get; set; }
        public string Modalidade { get; set; }
    }

}