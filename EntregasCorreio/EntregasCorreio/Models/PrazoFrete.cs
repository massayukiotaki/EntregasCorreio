using System.Text.Json;
using System.Text.Json.Serialization;

namespace EntregasCorreio.Models
{
    public class PrazoFrete
    {
        public DateTime DataMaxEntrega { get; set; }
    }

    public class PrazoFreteConverter : JsonConverter<PrazoFrete>
    {
        public override PrazoFrete Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Inicializa o objeto
            var prazoFrete = new PrazoFrete();

            // Lê o JSON manualmente
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    if (propertyName == "dataMaxima")
                    {
                        prazoFrete.DataMaxEntrega = DateTime.Parse(reader.GetString() ?? throw new JsonException());
                    }
                }
            }

            return prazoFrete;
        }

        public override void Write(Utf8JsonWriter writer, PrazoFrete value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("dataMaxima", value.DataMaxEntrega.ToString("o")); // ISO 8601
            writer.WriteEndObject();
        }
        }

    }
