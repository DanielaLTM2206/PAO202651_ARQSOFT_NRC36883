using System.Globalization;
using System.Text.RegularExpressions;

namespace ec.edu.monster.Util;

public static class ValidacionUtil
{
    // Signo opcional, hasta 15 dígitos enteros y hasta 10 decimales, con punto o coma.
    // [0-9] y no \d: en .NET \d también acepta dígitos de otros alfabetos (por ejemplo, ٣).
    private static readonly Regex PatronDecimal = new(@"^[+-]?[0-9]{1,15}([.,][0-9]{1,10})?$");

    // Mismo formato, pero aceptando números a medio escribir ("", "-", "12.") mientras el usuario teclea
    private static readonly Regex PatronDecimalParcial = new(@"^[+-]?[0-9]{0,15}([.,][0-9]{0,10})?$");

    /// <summary>
    /// Convierte el texto ingresado en un número. Rechaza texto, notación científica (1e5),
    /// NaN, Infinity y formatos con separadores de miles (1,000.5).
    /// </summary>
    public static double InterpretarDecimal(string? texto)
    {
        string valor = texto?.Trim() ?? string.Empty;

        if (valor.Length == 0)
        {
            throw new ArgumentException("Ingrese el valor a convertir.");
        }
        if (!PatronDecimal.IsMatch(valor))
        {
            throw new ArgumentException(
                "Ingrese solo números (máximo 15 enteros y 10 decimales), por ejemplo 25 o 12.5");
        }
        return double.Parse(valor.Replace(',', '.'), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Indica si el texto puede quedar en la caja del valor mientras se escribe.
    /// En el celular el texto cambia completo (teclado, pegar, autocompletar), así que se valida
    /// el texto entero: si no es un número a medio escribir, la vista restaura el texto anterior.
    /// </summary>
    public static bool EsTextoNumericoParcial(string? texto)
    {
        return PatronDecimalParcial.IsMatch(texto ?? string.Empty);
    }
}
