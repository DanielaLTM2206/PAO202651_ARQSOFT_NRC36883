using ec.edu.monster.Controlador;
using ec.edu.monster.Modelo;
using ec.edu.monster.Util;

namespace ec.edu.monster.Vista;

/// <summary>
/// Historial compartido de conversiones: lo guarda el servidor, así que incluye las conversiones
/// de todos los clientes (consola, escritorio, web y móvil). Los errores del servicio los maneja
/// ConversionVista, que decide si hay que volver al inicio de sesión.
/// </summary>
public class HistorialVista
{
    private readonly MensajesModelo mensajes = new();
    private readonly ConversionControlador conversionControlador;

    public HistorialVista(ConversionControlador conversionControlador)
    {
        this.conversionControlador = conversionControlador;
    }

    public async Task MostrarAsync()
    {
        IReadOnlyList<RegistroConversion> historial = await conversionControlador.ObtenerHistorialAsync();

        mensajes.MostrarTitulo($"HISTORIAL DE CONVERSIONES ({historial.Count})");
        if (historial.Count == 0)
        {
            mensajes.MostrarAviso("  Aún no hay conversiones registradas en el servidor.");
            return;
        }

        mensajes.MostrarDetalle("  #    Fecha y hora         Tipo         Conversión");
        for (int indice = 0; indice < historial.Count; indice++)
        {
            RegistroConversion registro = historial[indice];
            mensajes.MostrarMensaje($"  {indice + 1,-4} {FormatoUtil.FormatearFechaHora(registro.FechaHora)}  "
                    + $"{registro.Tipo.NombreVisible(),-12} {FormatoUtil.FormatearConversion(registro)}");
        }
        mensajes.MostrarDetalle("  Incluye las conversiones de todos los clientes conectados a este servidor.");
    }

    public async Task BorrarAsync()
    {
        mensajes.MostrarTitulo("BORRAR HISTORIAL");
        mensajes.MostrarAviso("  Se borrarán las conversiones de todos los clientes conectados a este servidor.");
        if (!EntradaUtil.LeerSiNo("  ¿Desea borrar el historial?"))
        {
            mensajes.MostrarAviso("  No se borró el historial.");
            return;
        }
        mensajes.MostrarExito(await conversionControlador.LimpiarHistorialAsync());
    }
}
