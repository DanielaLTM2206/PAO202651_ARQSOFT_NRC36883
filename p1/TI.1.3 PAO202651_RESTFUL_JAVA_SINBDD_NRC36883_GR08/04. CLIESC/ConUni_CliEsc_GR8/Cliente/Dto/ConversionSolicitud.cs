using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

public class ConversionSolicitud
{
    [JsonPropertyName("Valor")]
    public double Valor { get; set; }

    [JsonPropertyName("UnidadOrigen")]
    public string UnidadOrigen { get; set; } = string.Empty;

    [JsonPropertyName("UnidadDestino")]
    public string UnidadDestino { get; set; } = string.Empty;
}
