using System.Globalization;
using System.Text.RegularExpressions;

namespace ec.edu.monster.Util;

/// <summary>
/// Lectura de datos desde el teclado. Cada método repite la pregunta hasta recibir un dato válido.
/// </summary>
public static class EntradaUtil
{
    // [0-9] y no \d: en .NET \d también acepta dígitos de otros alfabetos (por ejemplo, ٣)
    private static readonly Regex PatronEntero = new(@"^[0-9]{1,9}$");
    private static readonly Regex PatronDecimal = new(@"^[+-]?[0-9]{1,15}([.,][0-9]{1,10})?$");

    public static string LeerTextoNoVacio(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = LeerLinea().Trim();
            if (texto.Length > 0)
            {
                return texto;
            }
            MostrarError("Entrada inválida. El valor no puede quedar vacío.");
        }
    }

    public static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string valor = LeerLinea().Trim();
            if (PatronEntero.IsMatch(valor))
            {
                return int.Parse(valor, CultureInfo.InvariantCulture);
            }
            MostrarError("Entrada inválida. Debe ingresar un número entero.");
        }
    }

    public static double LeerDecimal(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            try
            {
                return InterpretarDecimal(LeerLinea());
            }
            catch (ArgumentException ex)
            {
                MostrarError(ex.Message);
            }
        }
    }

    /// <summary>
    /// Acepta solo números decimales simples: signo opcional, hasta 15 dígitos enteros y
    /// hasta 10 decimales, separados por punto o coma (por ejemplo -12,5 o 1000.25).
    /// Rechaza texto, notación científica (1e5), NaN, Infinity y separadores de miles (1,000.5).
    /// </summary>
    public static double InterpretarDecimal(string? texto)
    {
        string valor = texto?.Trim() ?? string.Empty;

        if (valor.Length == 0)
        {
            throw new ArgumentException("Entrada inválida. El valor no puede quedar vacío.");
        }
        if (!PatronDecimal.IsMatch(valor))
        {
            throw new ArgumentException(
                "Entrada inválida. Ingrese solo números (máximo 15 enteros y 10 decimales), por ejemplo 25 o 12.5");
        }
        return double.Parse(valor.Replace(',', '.'), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);
    }

    public static bool LeerSiNo(string mensaje)
    {
        while (true)
        {
            string respuesta = LeerTextoNoVacio(mensaje + " (S/N): ").ToUpperInvariant();
            if (respuesta == "S")
            {
                return true;
            }
            if (respuesta == "N")
            {
                return false;
            }
            MostrarError("Responda S o N.");
        }
    }

    // ReadLine devuelve null si se cierra la entrada (Ctrl+Z): se termina el programa en lugar de repetir sin fin
    private static string LeerLinea()
    {
        return Console.ReadLine() ?? throw new EndOfStreamException("Se cerró la entrada de la consola.");
    }

    private static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  [ERROR] " + mensaje);
        Console.ResetColor();
    }
}
