using ec.edu.monster.Cliente;

namespace ec.edu.monster.Sesion;

/// <summary>
/// Datos de la sesión del navegador de cada usuario: token y nombre de usuario.
/// Viven en memoria del servidor web mientras la sesión está activa (no hay base de datos).
/// El historial de conversiones no se guarda aquí: es compartido y lo guarda el servidor Java.
/// </summary>
public class AlmacenSesion : IAlmacenToken
{
    private const string ClaveToken = "Token";
    private const string ClaveUsuario = "Usuario";

    private readonly IHttpContextAccessor accesoContexto;

    public AlmacenSesion(IHttpContextAccessor accesoContexto)
    {
        this.accesoContexto = accesoContexto;
    }

    private ISession Sesion => accesoContexto.HttpContext?.Session
            ?? throw new InvalidOperationException("No hay una petición HTTP activa.");

    public string? Token
    {
        get => Sesion.GetString(ClaveToken);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Sesion.Remove(ClaveToken);
            }
            else
            {
                Sesion.SetString(ClaveToken, value);
            }
        }
    }

    public string? Usuario
    {
        get => Sesion.GetString(ClaveUsuario);
        set => Sesion.SetString(ClaveUsuario, value ?? string.Empty);
    }

    public bool HaySesionActiva => !string.IsNullOrWhiteSpace(Token);

    public void Limpiar()
    {
        Sesion.Clear();
    }
}
