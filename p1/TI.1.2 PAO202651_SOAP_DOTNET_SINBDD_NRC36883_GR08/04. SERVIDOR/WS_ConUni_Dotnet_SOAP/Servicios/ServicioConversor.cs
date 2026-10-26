using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades;

namespace ec.edu.monster.Servicios;

/// <summary>
/// Envuelve un conversor y valida que el valor y el resultado sean números finitos.
/// </summary>
public class ServicioConversor<T> where T : struct, Enum
{
    private readonly IConversor<T> conversor;

    public ServicioConversor(IConversor<T> conversor)
    {
        this.conversor = conversor ?? throw new ArgumentNullException(nameof(conversor));
    }

    public double Convertir(double valor, T origen, T destino)
    {
        if (!double.IsFinite(valor))
        {
            throw new ExcepcionValidacion("El valor debe ser un número finito");
        }

        double resultado = conversor.Convertir(valor, origen, destino);

        if (!double.IsFinite(resultado))
        {
            throw new ExcepcionValidacion("El resultado excede el rango numérico soportado");
        }
        return resultado;
    }
}
