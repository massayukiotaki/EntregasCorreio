namespace EntregasCorreio.Models
{
public class CorreiosRateResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("preco")]
    public float Preco { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("prazo")]
    public DateTime Prazo { get; set; }

    public CorreiosRateResponse(float precoFrete, DateTime prazoFrete)
    {
        Preco = precoFrete;
        Prazo = prazoFrete;
    }
}
}