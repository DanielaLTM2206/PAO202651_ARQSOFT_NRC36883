using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades.Enums;

namespace ec.edu.monster.Modelos.Utilidades.Mapeadores;

/// <summary>
/// Traduce el texto recibido en el mensaje SOAP (por ejemplo "metro" o "METRO") a la unidad del enum.
/// </summary>
public static class UnidadMapper
{
    public static UnidadLongitud ALongitud(string? unidad) => Convertir<UnidadLongitud>(unidad, "longitud");

    public static UnidadMasa AMasa(string? unidad) => Convertir<UnidadMasa>(unidad, "masa");

    public static UnidadTemperatura ATemperatura(string? unidad) => Convertir<UnidadTemperatura>(unidad, "temperatura");

    private static T Convertir<T>(string? unidad, string categoria) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(unidad))
        {
            throw new ExcepcionValidacion("La unidad de " + categoria + " es obligatoria");
        }

        // Se compara por nombre: Enum.TryParse aceptaría también números como "2"
        string? nombre = Enum.GetNames<T>()
                .FirstOrDefault(n => n.Equals(unidad.Trim(), StringComparison.OrdinalIgnoreCase));

        if (nombre is null)
        {
            throw new ExcepcionValidacion("Unidad de " + categoria + " inválida: " + unidad);
        }
        return Enum.Parse<T>(nombre);
    }
}
