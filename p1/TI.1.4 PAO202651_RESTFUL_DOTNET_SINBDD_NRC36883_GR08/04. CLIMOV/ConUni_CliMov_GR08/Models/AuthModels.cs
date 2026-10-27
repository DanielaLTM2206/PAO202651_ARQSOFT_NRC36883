using System.Text.Json.Serialization;

namespace UniversalConverter.Client.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;

        [JsonPropertyName("clave")]
        public string Clave { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        [JsonPropertyName("exitoso")]
        public bool Exitoso { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("usuario")]
        public string? Usuario { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }
    }

    public class CambiarClaveRequest
    {
        [JsonPropertyName("claveActual")]
        public string ClaveActual { get; set; } = string.Empty;

        [JsonPropertyName("claveNueva")]
        public string ClaveNueva { get; set; } = string.Empty;
    }

    public class GenericApiResponse
    {
        [JsonPropertyName("exitoso")]
        public bool Exitoso { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }
    }
}
