using ec.edu.monster.Excepciones;
using ec.edu.monster.Modelos.Utilidades.Enums;

namespace ec.edu.monster.Servicios;

/// <summary>
/// Catálogo de unidades soportadas, obtenido directamente de los enums.
/// </summary>
public static class ServicioUnidades
{
    public static readonly IReadOnlyList<string> Categorias = ["longitud", "masa", "temperatura"];

    public static List<string> ObtenerUnidades(string? categoria)
    {
        string clave = NormalizarCategoria(categoria);

        return clave switch
        {
            "longitud" => [.. Enum.GetNames<UnidadLongitud>()],
            "masa" => [.. Enum.GetNames<UnidadMasa>()],
            "temperatura" => [.. Enum.GetNames<UnidadTemperatura>()],
            _ => throw new ExcepcionValidacion("Categoría no soportada: " + categoria
                    + ". Categorías válidas: " + string.Join(", ", Categorias))
        };
    }

    public static string NormalizarCategoria(string? categoria)
    {
        return categoria?.Trim().ToLowerInvariant() ?? string.Empty;
    }
}
