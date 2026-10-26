using System.Collections.Concurrent;
using ec.edu.monster.Excepciones;

namespace ec.edu.monster.Seguridad;

/// <summary>
/// Tokens activos en memoria. Cada token vence tras 30 minutos sin uso;
/// cada uso válido renueva el plazo.
/// </summary>
public static class AdministradorTokens
{
    public static readonly TimeSpan DuracionSesion = TimeSpan.FromMinutes(30);

    // token -> instante de vencimiento (UTC)
    private static readonly ConcurrentDictionary<string, DateTime> tokensActivos = new();

    public static string GenerarToken()
    {
        DateTime ahora = DateTime.UtcNow;
        foreach (var (token, vencimiento) in tokensActivos)
        {
            if (vencimiento < ahora)
            {
                tokensActivos.TryRemove(token, out _);
            }
        }

        string nuevoToken = Guid.NewGuid().ToString();
        tokensActivos[nuevoToken] = ahora + DuracionSesion;
        return nuevoToken;
    }

    public static void ValidarToken(string? token)
    {
        if (!EsTokenValido(token, out string mensajeError))
        {
            throw new ExcepcionAutenticacion(mensajeError);
        }
    }

    // Igual que ValidarToken, pero sin lanzar excepciones: lo usa InspectorAutenticacion en cada solicitud
    public static bool EsTokenValido(string? token, out string mensajeError)
    {
        mensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(token))
        {
            mensajeError = "Acceso denegado: No se encontró el token. Inicie sesión primero.";
            return false;
        }

        if (!tokensActivos.TryGetValue(token, out DateTime vencimiento))
        {
            mensajeError = "Acceso denegado: Token inválido.";
            return false;
        }

        DateTime ahora = DateTime.UtcNow;
        if (vencimiento < ahora)
        {
            tokensActivos.TryRemove(token, out _);
            mensajeError = "Acceso denegado: La sesión ha expirado. Inicie sesión nuevamente.";
            return false;
        }

        tokensActivos[token] = ahora + DuracionSesion;
        return true;
    }

    // Solo para las pruebas: simula que el token ya venció
    internal static void Expirar(string token)
    {
        tokensActivos[token] = DateTime.UtcNow.AddSeconds(-1);
    }
}
