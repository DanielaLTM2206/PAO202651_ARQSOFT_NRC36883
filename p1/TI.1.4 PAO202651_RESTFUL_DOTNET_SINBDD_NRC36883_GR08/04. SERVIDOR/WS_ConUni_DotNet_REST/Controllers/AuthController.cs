using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;
using WS_ConUni_DotNet_REST.Services;

namespace WS_ConUni_DotNet_REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IServicioSeguridad _seguridad;

        public AuthController(IServicioSeguridad seguridad)
        {
            _seguridad = seguridad;
        }

        /// <summary>
        /// RF-01: Autenticación de Usuario y Emisión de Token de Seguridad
        /// </summary>
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            var res = _seguridad.Autenticar(request.Usuario, request.Clave);
            if (!res.Exitoso)
            {
                return Unauthorized(res);
            }
            return Ok(res);
        }

        /// <summary>
        /// RF-08: Cambio de contraseña de usuario en memoria
        /// </summary>
        [HttpPost("cambiar-clave")]
        public ActionResult<RespuestaBase> CambiarClave([FromBody] CambiarClaveRequest request)
        {
            string? token = ObtenerTokenHeader();
            var res = _seguridad.CambiarClave(token, request.ClaveActual, request.ClaveNueva);

            if (!res.Exitoso)
            {
                if (res.Codigo == ErrorConstants.AUTH_TOKEN_INVALIDO)
                    return Unauthorized(res);

                return BadRequest(res);
            }

            return Ok(res);
        }

        /// <summary>
        /// Valida si el token actual sigue activo y válido
        /// </summary>
        [HttpGet("validar")]
        public ActionResult<object> ValidarToken()
        {
            string? token = ObtenerTokenHeader();
            if (_seguridad.ValidarToken(token, out string usuario))
            {
                return Ok(new { Valido = true, Usuario = usuario, Mensaje = "Token válido" });
            }

            return Unauthorized(new { Valido = false, Mensaje = "Token inválido o expirado" });
        }

        /// <summary>
        /// Cierra la sesión revocando el token
        /// </summary>
        [HttpPost("logout")]
        public ActionResult<RespuestaBase> Logout()
        {
            string? token = ObtenerTokenHeader();
            _seguridad.CerrarSesion(token);
            return Ok(RespuestaBase.Ok("Sesión finalizada"));
        }

        private string? ObtenerTokenHeader()
        {
            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var val = authHeader.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(val))
                {
                    return val.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase)
                        ? val.Substring(7).Trim()
                        : val.Trim();
                }
            }
            return null;
        }
    }
}
