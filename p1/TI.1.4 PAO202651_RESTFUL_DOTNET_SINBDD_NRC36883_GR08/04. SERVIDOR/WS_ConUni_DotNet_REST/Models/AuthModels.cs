using System;

namespace WS_ConUni_DotNet_REST.Models
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

        public static LoginResponse Exito(string token, string usuario, DateTime expiracion)
        {
            return new LoginResponse
            {
                Exitoso = true,
                Codigo = "AUTH_OK",
                Mensaje = "Autenticación exitosa",
                Token = token,
                Usuario = usuario,
                FechaExpiracion = expiracion
            };
        }

        public static LoginResponse Fallo(string codigo, string mensaje)
        {
            return new LoginResponse
            {
                Exitoso = false,
                Codigo = codigo,
                Mensaje = mensaje,
                Token = null,
                Usuario = null,
                FechaExpiracion = null
            };
        }
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

        public static RespuestaBase Ok(string mensaje = "Operación exitosa")
        {
            return new RespuestaBase { Exitoso = true, Codigo = "OK_200", Mensaje = mensaje };
        }

        public static RespuestaBase Error(string codigo, string mensaje)
        {
            return new RespuestaBase { Exitoso = false, Codigo = codigo, Mensaje = mensaje };
        }
    }
}
