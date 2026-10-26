namespace ec.edu.monster.Modelo;

/// <summary>
/// Textos y formato de la consola: banner, títulos y mensajes de éxito, aviso y error con color.
/// </summary>
public class MensajesModelo
{
    private const int Ancho = 66;

    // Arte ASCII ("ConUni" + "GR08")
    private static readonly string[] Logo =
    [
        @"  ____            _   _       _     ____ ____   ___   ___  ",
        @" / ___|___  _ __ | | | |_ __ (_)   / ___|  _ \ / _ \ ( _ ) ",
        @"| |   / _ \| '_ \| | | | '_ \| |  | |  _| |_) | | | |/ _ \ ",
        @"| |__| (_) | | | | |_| | | | | |  | |_| |  _ <| |_| | (_) |",
        @" \____\___/|_| |_|\___/|_| |_|_|   \____|_| \_\\___/ \___/ "
    ];

    public void MostrarBanner()
    {
        Escribir(Borde('='), ConsoleColor.Cyan);
        foreach (string linea in Logo)
        {
            Escribir(LineaCentrada(linea), ConsoleColor.Cyan);
        }
        Escribir(Borde('='), ConsoleColor.Cyan);
        Escribir(LineaCentrada("SISTEMA DE CONVERSIÓN DE UNIDADES"), ConsoleColor.Cyan);
        Escribir(LineaCentrada("Cliente Consola  -  Servicio SOAP .NET"), ConsoleColor.Cyan);
        Escribir(LineaCentrada("Longitud  |  Masa  |  Temperatura"), ConsoleColor.Cyan);
        Escribir(Borde('='), ConsoleColor.Cyan);
    }

    public void MostrarTitulo(string texto)
    {
        Console.WriteLine();
        Escribir(Borde('-'), ConsoleColor.Cyan);
        Escribir(LineaCentrada(texto), ConsoleColor.Cyan);
        Escribir(Borde('-'), ConsoleColor.Cyan);
    }

    public void MostrarMensaje(string mensaje)
    {
        Console.WriteLine(mensaje);
    }

    public void MostrarDetalle(string mensaje)
    {
        Escribir(mensaje, ConsoleColor.DarkGray);
    }

    public void MostrarAviso(string mensaje)
    {
        Escribir(mensaje, ConsoleColor.Yellow);
    }

    public void MostrarExito(string mensaje)
    {
        Escribir("  [OK] " + mensaje, ConsoleColor.Green);
    }

    public void MostrarError(string mensaje)
    {
        Escribir("  [ERROR] " + mensaje, ConsoleColor.Red);
    }

    private static void Escribir(string texto, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(texto);
        Console.ResetColor();
    }

    private static string Borde(char relleno)
    {
        return "+" + new string(relleno, Ancho - 2) + "+";
    }

    private static string LineaCentrada(string texto)
    {
        int espacio = Ancho - 4 - texto.Length;
        int izquierda = Math.Max(espacio / 2, 0);
        int derecha = Math.Max(espacio - izquierda, 0);
        return "| " + new string(' ', izquierda) + texto + new string(' ', derecha) + " |";
    }
}
