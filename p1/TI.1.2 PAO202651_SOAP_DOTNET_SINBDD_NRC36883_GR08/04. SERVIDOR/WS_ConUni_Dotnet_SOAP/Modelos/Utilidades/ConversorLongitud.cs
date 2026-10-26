using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades.Enums;

namespace ec.edu.monster.Modelos.Utilidades;

/// <summary>
/// Convierte pasando por la unidad base (metro): valor * factor de origen / factor de destino.
/// </summary>
public class ConversorLongitud : IConversor<UnidadLongitud>
{
    private static readonly Dictionary<UnidadLongitud, double> factores = new()
    {
        [UnidadLongitud.MILIMETRO] = 0.001,
        [UnidadLongitud.CENTIMETRO] = 0.01,
        [UnidadLongitud.METRO] = 1.0,
        [UnidadLongitud.KILOMETRO] = 1000.0,
        [UnidadLongitud.YARDA] = 0.9144
    };

    public double Convertir(double valor, UnidadLongitud unidadOrigen, UnidadLongitud unidadDestino)
    {
        if (!factores.ContainsKey(unidadOrigen) || !factores.ContainsKey(unidadDestino))
        {
            throw new ArgumentException("Unidad no soportada");
        }
        if (valor < 0)
        {
            throw new ExcepcionValidacion("La longitud no puede ser negativa");
        }

        double valorEnBase = valor * factores[unidadOrigen];
        return valorEnBase / factores[unidadDestino];
    }
}
