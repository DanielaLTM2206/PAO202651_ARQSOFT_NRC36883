using System;
using System.Collections.Concurrent;
using WS_ConUni_DotNet_REST.Constants;
using WS_ConUni_DotNet_REST.Models;

namespace WS_ConUni_DotNet_REST.Services
{
    public interface IServicioSeguridad
    {
        LoginResponse Autenticar(string usuario, string clave);
        bool ValidarToken(string? token, out string usuario);
        RespuestaBase CambiarClave(string? token, string claveActual, string claveNueva);
        int ObtenerIntentosFallidos(string usuario);
        void CerrarSesion(string? token);
    }

    public class ServicioSeguridad : IServicioSeguridad
    {
        // Almacenamiento en memoria (SINBDD)
        private static readonly ConcurrentDictionary<string, string> _usuariosClaves = new(StringComparer.OrdinalIgnoreCase)
        {
            ["monster"] = "monster9",
            ["admin"] = "admin123"
        };

        // Tokens activos: Token -> (Usuario, Expiracion)
        private static readonly ConcurrentDictionary<string, (string Usuario, DateTime Expiracion)> _tokensActivos = new();

        // Control de intentos fallidos
        // Control de intentos fallidos con desbloqueo temporal automático (30 segundos)
        private static readonly ConcurrentDictionary<string, (int Intentos, DateTime BloqueadoHasta)> _intentosFallidos = new(StringComparer.OrdinalIgnoreCase);

        private const int MAX_INTENTOS = 3;
        private static readonly TimeSpan DURACION_BLOQUEO = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan DURACION_TOKEN = TimeSpan.FromHours(8);

        public LoginResponse Autenticar(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                return LoginResponse.Fallo(ErrorConstants.AUTH_CAMPOS_REQUERIDOS, "Usuario y clave son requeridos.");
            }

            usuario = usuario.Trim('\uFEFF', ' ', '\r', '\n', '\t');
            clave = clave.Trim('\uFEFF', ' ', '\r', '\n', '\t');

            // Verificar si el usuario está temporalmente bloqueado con expiración automática
            if (_intentosFallidos.TryGetValue(usuario, out var fallosInfo))
            {
                if (fallosInfo.Intentos >= MAX_INTENTOS)
                {
                    if (DateTime.UtcNow < fallosInfo.BloqueadoHasta)
                    {
                        int segs = Math.Max(1, (int)(fallosInfo.BloqueadoHasta - DateTime.UtcNow).TotalSeconds);
                        return LoginResponse.Fallo(
                            ErrorConstants.AUTH_CREDENCIALES_INVALIDAS,
                            $"Cuenta bloqueada temporalmente. Espere {segs} segundo(s) o reinicie el servidor.");
                    }
                    else
                    {
                        // Ya pasó el tiempo de bloqueo -> desbloqueo automático
                        _intentosFallidos.TryRemove(usuario, out _);
                    }
                }
            }

            // Validar credenciales exactamente contra la contraseña almacenada en memoria (sin atajos quemados)
            if (_usuariosClaves.TryGetValue(usuario, out string? claveEsperada) && 
                string.Equals(claveEsperada, clave, StringComparison.Ordinal))
            {
                // Resetear intentos fallidos
                _intentosFallidos.TryRemove(usuario, out _);

                // Generar token seguro en memoria
                string token = $"mst_{Guid.NewGuid():N}_{DateTime.UtcNow.Ticks}";
                DateTime expiracion = DateTime.UtcNow.Add(DURACION_TOKEN);

                _tokensActivos[token] = (usuario.ToLowerInvariant(), expiracion);

                return LoginResponse.Exito(token, usuario.ToLowerInvariant(), expiracion);
            }
            else
            {
                var ahora = DateTime.UtcNow;
                int nuevosFallos = 1;
                if (_intentosFallidos.TryGetValue(usuario, out var prev) && ahora < prev.BloqueadoHasta)
                {
                    nuevosFallos = prev.Intentos + 1;
                }

                var bloqueadoHasta = nuevosFallos >= MAX_INTENTOS 
                    ? ahora.Add(DURACION_BLOQUEO) 
                    : ahora.Add(TimeSpan.FromMinutes(5));

                _intentosFallidos[usuario] = (nuevosFallos, bloqueadoHasta);

                int restantes = MAX_INTENTOS - nuevosFallos;
                string mensaje = restantes > 0
                    ? $"Credenciales incorrectas. Te quedan {restantes} intento(s)."
                    : $"Superó los {MAX_INTENTOS} intentos. Cuenta bloqueada por 30 segundos.";

                return LoginResponse.Fallo(ErrorConstants.AUTH_CREDENCIALES_INVALIDAS, mensaje);
            }
        }

        public bool ValidarToken(string? token, out string usuario)
        {
            usuario = string.Empty;

            if (string.IsNullOrWhiteSpace(token))
                return false;

            // Limpiar prefijo "Bearer " si viene en el header
            token = token.Trim();
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring(7).Trim();
            }

            if (_tokensActivos.TryGetValue(token, out var sesion))
            {
                if (DateTime.UtcNow <= sesion.Expiracion)
                {
                    usuario = sesion.Usuario;
                    return true;
                }
                else
                {
                    // Token expirado, eliminarlo
                    _tokensActivos.TryRemove(token, out _);
                }
            }

            return false;
        }

        public RespuestaBase CambiarClave(string? token, string claveActual, string claveNueva)
        {
            if (!ValidarToken(token, out string usuario))
            {
                return RespuestaBase.Error(ErrorConstants.AUTH_TOKEN_INVALIDO, "Token no válido o sesión expirada.");
            }

            if (string.IsNullOrWhiteSpace(claveActual) || string.IsNullOrWhiteSpace(claveNueva))
            {
                return RespuestaBase.Error(ErrorConstants.AUTH_CAMPOS_REQUERIDOS, "Debe ingresar la clave actual y la nueva clave.");
            }

            claveActual = claveActual.Trim('\uFEFF', ' ', '\r', '\n', '\t');
            claveNueva = claveNueva.Trim('\uFEFF', ' ', '\r', '\n', '\t');

            if (!_usuariosClaves.TryGetValue(usuario, out string? claveGuardada) || 
                !string.Equals(claveGuardada, claveActual, StringComparison.Ordinal))
            {
                return RespuestaBase.Error(ErrorConstants.AUTH_CLAVE_ACTUAL_INCORRECTA, "La contraseña actual es incorrecta.");
            }

            if (claveNueva.Length < 4)
            {
                return RespuestaBase.Error(ErrorConstants.AUTH_CLAVE_NUEVA_INVALIDA, "La nueva contraseña debe tener al menos 4 caracteres.");
            }

            if (string.Equals(claveActual, claveNueva, StringComparison.Ordinal))
            {
                return RespuestaBase.Error(ErrorConstants.AUTH_CLAVE_NUEVA_INVALIDA, "La nueva contraseña no puede ser igual a la actual.");
            }

            _usuariosClaves[usuario] = claveNueva;

            return RespuestaBase.Ok($"Contraseña de '{usuario}' actualizada exitosamente en memoria.");
        }

        public int ObtenerIntentosFallidos(string usuario)
        {
            if (_intentosFallidos.TryGetValue(usuario.Trim(), out var info))
            {
                return DateTime.UtcNow < info.BloqueadoHasta ? info.Intentos : 0;
            }
            return 0;
        }

        public void CerrarSesion(string? token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    token = token.Substring(7).Trim();

                _tokensActivos.TryRemove(token, out _);
            }
        }
    }
}
