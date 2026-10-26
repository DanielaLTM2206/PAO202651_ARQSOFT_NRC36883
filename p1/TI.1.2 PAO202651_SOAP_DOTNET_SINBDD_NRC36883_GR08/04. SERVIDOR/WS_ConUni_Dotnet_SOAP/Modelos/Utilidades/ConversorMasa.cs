using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades.Enums;

namespace ec.edu.monster.Modelos.Utilidades;

/// <summary>
/// Convierte pasando por la unidad base (gramo): valor * factor de origen / factor de destino.
/// </summary>
public class ConversorMasa : IConversor<UnidadMasa>
{
    private static readonly Dictionary<UnidadMasa, double> factores = new()
    {
        [UnidadMasa.MILIGRAMO] = 0.001,
        [UnidadMasa.GRAMO] = 1.0,
        [UnidadMasa.KILOGRAMO] = 1000.0,
        [UnidadMasa.TONELADA] = 1_000_000.0,
        [UnidadMasa.ONZA] = 28.3495
    };

    public double Convertir(double valor, UnidadMasa unidadOrigen, UnidadMasa unidadDestino)
    {
        if (!factores.ContainsKey(unidadOrigen) || !factores.ContainsKey(unidadDestino))
        {
            throw new ArgumentException("Unidad no soportada");
        }
        if (valor < 0)
        {
            throw new ExcepcionValidacion("La masa no puede ser negativa");
        }

        double valorEnBase = valor * factores[unidadOrigen];
        return valorEnBase / factores[unidadDestino];
    }
}
