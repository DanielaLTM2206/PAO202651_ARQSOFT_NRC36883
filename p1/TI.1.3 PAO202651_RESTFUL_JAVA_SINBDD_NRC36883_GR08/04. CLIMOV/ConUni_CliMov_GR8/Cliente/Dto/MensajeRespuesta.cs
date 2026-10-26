using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

/// <summary>
/// Respuesta del servidor con un único mensaje: confirmaciones y errores (400, 401, 500).
/// </summary>
public class MensajeRespuesta
{
    [JsonPropertyName("Mensaje")]
    public string? Mensaje { get; set; }
}
