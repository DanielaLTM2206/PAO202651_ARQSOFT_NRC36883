using ec.edu.monster.Excepciones;

namespace ec.edu.monster.Seguridad;

/// <summary>
/// Credenciales del único usuario del sistema, en memoria (no hay base de datos).
/// </summary>
public static class AdministradorCredenciales
{
    private const string Usuario = "Monster";
    private const string ContraseniaInicial = "Monster9";

    private static string contrasenia = ContraseniaInicial;
    private static readonly Lock candado = new();

    public static bool ValidarCredenciales(string usuario, string contraseniaIngresada)
    {
        lock (candado)
        {
            return Usuario == usuario && contrasenia == contraseniaIngresada;
        }
    }

    /// <summary>
    /// Una contraseña actual incorrecta se trata como error de validación (400) y no
    /// de autenticación (401): ante un 401 los clientes descartan el token y vuelven al login.
    /// </summary>
    public static void CambiarContrasenia(string contraseniaActual, string? contraseniaNueva)
    {
        lock (candado)
        {
            if (contrasenia != contraseniaActual)
            {
                throw new ExcepcionValidacion("La contraseña actual es incorrecta");
            }
            if (string.IsNullOrWhiteSpace(contraseniaNueva))
            {
                throw new ExcepcionValidacion("La nueva contraseña es obligatoria");
            }
            if (contraseniaNueva == contrasenia)
            {
                throw new ExcepcionValidacion("La nueva contraseña debe ser diferente a la actual");
            }

            contrasenia = contraseniaNueva;
        }
    }

    // Solo para las pruebas: deja la contraseña por defecto para no afectar a otras pruebas
    internal static void Restablecer()
    {
        lock (candado)
        {
            contrasenia = ContraseniaInicial;
        }
    }
}
