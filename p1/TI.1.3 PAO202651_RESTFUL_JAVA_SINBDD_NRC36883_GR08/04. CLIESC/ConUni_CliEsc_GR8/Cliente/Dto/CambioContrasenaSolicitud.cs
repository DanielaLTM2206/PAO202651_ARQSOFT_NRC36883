using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

public class CambioContrasenaSolicitud
{
    [JsonPropertyName("ContrasenaActual")]
    public string ContrasenaActual { get; set; } = string.Empty;

    [JsonPropertyName("NuevaContrasena")]
    public string NuevaContrasena { get; set; } = string.Empty;
}
