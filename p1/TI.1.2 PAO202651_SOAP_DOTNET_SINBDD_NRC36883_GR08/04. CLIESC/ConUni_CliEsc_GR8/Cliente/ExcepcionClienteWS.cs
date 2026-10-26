namespace ec.edu.monster.Cliente;

/// <summary>
/// Error al consumir el servicio. Guarda el código que trae el fault ErrorServicio del servidor
/// para que la vista pueda distinguir un problema de sesión (401) de un dato inválido (400).
/// Código 0 indica que no hubo respuesta del servidor.
/// </summary>
public class ExcepcionClienteWS : Exception
{
    public const int SinConexion = 0;

    public int CodigoEstado { get; }

    public ExcepcionClienteWS(int codigoEstado, string mensaje) : base(mensaje)
    {
        CodigoEstado = codigoEstado;
    }

    public bool EsErrorAutenticacion => CodigoEstado == 401;

    public bool EsErrorConexion => CodigoEstado == SinConexion;
}
