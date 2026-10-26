namespace ec.edu.monster.Modelo;

public enum TipoConversion
{
    Longitud,
    Masa,
    Temperatura
}

public static class TipoConversionExtensiones
{
    public static string NombreVisible(this TipoConversion tipo) => tipo switch
    {
        TipoConversion.Longitud => "Longitud",
        TipoConversion.Masa => "Masa",
        TipoConversion.Temperatura => "Temperatura",
        _ => throw new ArgumentOutOfRangeException(nameof(tipo))
    };

    // Categoría tal como la espera la operación SOAP ConsultarUnidadesPorCategoria
    public static string Categoria(this TipoConversion tipo) => tipo switch
    {
        TipoConversion.Longitud => "longitud",
        TipoConversion.Masa => "masa",
        TipoConversion.Temperatura => "temperatura",
        _ => throw new ArgumentOutOfRangeException(nameof(tipo))
    };
}
