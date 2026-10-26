using System.Text.Json.Serialization;

namespace ec.edu.monster.Cliente.Dto;

public class LoginRespuesta
{
    [JsonPropertyName("Token")]
    public string Token { get; set; } = string.Empty;
}
