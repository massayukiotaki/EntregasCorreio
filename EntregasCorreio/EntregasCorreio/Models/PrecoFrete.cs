using System.Text.Json.Serialization;
using System.Text.Json;

namespace EntregasCorreio.Models
{
    public class PrecoFrete
    {
        public float PcFinal { get; set; }
    }

    public class PrecoFreteConverter : JsonConverter<PrecoFrete>
    {
        public override PrecoFrete Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var precoFrete = new PrecoFrete();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    if (propertyName == "pcFinal")
                    {
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            // Lê o valor como string e adiciona o ponto decimal manualmente
                            string rawValue = reader.GetString();
                            if (!string.IsNullOrEmpty(rawValue))
                            {
                                // Insere o ponto decimal antes das duas últimas casas
                                rawValue = rawValue.Insert(rawValue.Length - 2, ".");
                                precoFrete.PcFinal = float.Parse(rawValue);
                            }
                        }
                        else if (reader.TokenType == JsonTokenType.Number)
                        {
                            precoFrete.PcFinal = reader.GetSingle();
                        }
                        else
                        {
                            throw new JsonException("Formato inesperado para 'pcFinal'.");
                        }
                    }
                }
            }

            return precoFrete;
        }

        public override void Write(Utf8JsonWriter writer, PrecoFrete value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            // Formata o número para JSON no formato adequado
            writer.WriteString("pcFinal", value.PcFinal.ToString("F2").Replace(",", "."));
            writer.WriteEndObject();
        }
    }
}
