using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

public class ConversionRespuesta
{
    [JsonPropertyName("ValorConvertido")]
    public double ValorConvertido { get; set; }
}
