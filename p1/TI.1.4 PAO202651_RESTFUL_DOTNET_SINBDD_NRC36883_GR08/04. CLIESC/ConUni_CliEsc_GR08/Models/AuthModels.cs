using System;

namespace ConUni_CliEsc_GR08.Models
{
    public class LoginRequest
    {
        public string Usuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Exitoso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? Usuario { get; set; }
        public DateTime? FechaExpiracion { get; set; }
    }

    public class CambiarClaveRequest
    {
        public string ClaveActual { get; set; } = string.Empty;
        public string ClaveNueva { get; set; } = string.Empty;
    }

    public class RespuestaBase
    {
        public bool Exitoso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}
