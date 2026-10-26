namespace ec.edu.monster.Excepciones;

/// <summary>
/// Datos de entrada faltantes, inválidos o inconsistentes. Se responde con un fault de código 400.
/// </summary>
public class ExcepcionValidacion : Exception
{
    public ExcepcionValidacion(string mensaje) : base(mensaje)
    {
    }
}
