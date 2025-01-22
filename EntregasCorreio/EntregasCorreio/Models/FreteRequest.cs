namespace EntregasCorreio.Models
{
    public class FreteRequest
    {
        public string CepOrigem { get; set; }
        public string CepDestino { get; set; }
        public float PesoObjeto { get; set; }
        public string Modalidade { get; set; }
    }
}
