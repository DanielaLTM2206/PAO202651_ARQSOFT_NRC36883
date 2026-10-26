using System.Globalization;
using ec.edu.monster.Modelos.Dto;

namespace ec.edu.monster.Servicios;

/// <summary>
/// Historial de conversiones compartido por todos los clientes (escritorio, web, móvil y consola).
/// Vive en memoria del servidor, igual que las credenciales y los tokens (no hay base de datos):
/// se pierde al reiniciar el servicio. Guarda las más recientes primero, como máximo CapacidadMaxima.
/// </summary>
public static class ServicioHistorial
{
    public const int CapacidadMaxima = 100;

    private static readonly LinkedList<RegistroConversionRespuesta> registros = new();
    private static readonly Lock candado = new();

    public static void Registrar(string tipo, double valor, string unidadOrigen, double resultado, string unidadDestino)
    {
        Registrar(DateTime.Now, tipo, valor, unidadOrigen, resultado, unidadDestino);
    }

    // La fecha se puede indicar en las pruebas para obtener registros predecibles
    public static void Registrar(DateTime fechaHora, string tipo, double valor,
                                 string unidadOrigen, double resultado, string unidadDestino)
    {
        var registro = new RegistroConversionRespuesta
        {
            FechaHora = fechaHora.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture),
            Tipo = tipo,
            Valor = valor,
            UnidadOrigen = unidadOrigen,
            Resultado = resultado,
            UnidadDestino = unidadDestino
        };

        lock (candado)
        {
            registros.AddFirst(registro);
            while (registros.Count > CapacidadMaxima)
            {
                registros.RemoveLast();
            }
        }
    }

    public static IReadOnlyList<RegistroConversionRespuesta> Obtener()
    {
        lock (candado)
        {
            return registros.ToList().AsReadOnly();
        }
    }

    public static void Limpiar()
    {
        lock (candado)
        {
            registros.Clear();
        }
    }
}
