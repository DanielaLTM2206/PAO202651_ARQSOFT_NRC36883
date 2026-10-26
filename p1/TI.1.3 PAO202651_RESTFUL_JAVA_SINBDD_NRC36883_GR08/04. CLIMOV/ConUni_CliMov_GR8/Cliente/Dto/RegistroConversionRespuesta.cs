using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

/// <summary>
/// Una conversión del historial compartido que guarda el servidor.
/// FechaHora llega en formato ISO-8601 (por ejemplo, 2026-10-18T19:28:36).
/// </summary>
public class RegistroConversionRespuesta
{
    [JsonPropertyName("FechaHora")]
    public string FechaHora { get; set; } = string.Empty;

    [JsonPropertyName("Tipo")]
    public string Tipo { get; set; } = string.Empty;

    [JsonPropertyName("Valor")]
    public double Valor { get; set; }

    [JsonPropertyName("UnidadOrigen")]
    public string UnidadOrigen { get; set; } = string.Empty;

    [JsonPropertyName("Resultado")]
    public double Resultado { get; set; }

    [JsonPropertyName("UnidadDestino")]
    public string UnidadDestino { get; set; } = string.Empty;
}
