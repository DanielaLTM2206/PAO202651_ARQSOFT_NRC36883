using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

public class LoginSolicitud
{
    [JsonPropertyName("Usuario")]
    public string Usuario { get; set; } = string.Empty;

    [JsonPropertyName("Contrasena")]
    public string Contrasena { get; set; } = string.Empty;
}
