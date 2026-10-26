namespace ec.edu.monster.Excepciones;

/// <summary>
/// Credenciales incorrectas o token ausente, inválido o expirado. Se responde con un fault de código 401.
/// </summary>
public class ExcepcionAutenticacion : Exception
{
    public ExcepcionAutenticacion(string mensaje) : base(mensaje)
    {
    }
}
