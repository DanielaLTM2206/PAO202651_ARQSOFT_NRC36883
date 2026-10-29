package ec.edu.monster.vista;

import ec.edu.monster.controlador.ConversionControlador;
import ec.edu.monster.modelo.MensajesModelo;
import ec.edu.monster.modelo.RegistroConversion;
import ec.edu.monster.util.EntradaUtil;
import ec.edu.monster.util.FormatoUtil;

import java.util.List;

/**
 * Historial compartido de conversiones: lo guarda el servidor, asi que incluye las conversiones
 * de todos los clientes (consola, escritorio, web y movil). Los errores del servicio los maneja
 * ConversionVista, que decide si hay que volver al inicio de sesion.
 */
public class HistorialVista {

    private final MensajesModelo mensajes = new MensajesModelo();
    private final ConversionControlador conversionControlador;

    public HistorialVista(ConversionControlador conversionControlador) {
        this.conversionControlador = conversionControlador;
    }

    public void mostrar() {
        List<RegistroConversion> historial = conversionControlador.obtenerHistorial();

        mensajes.mostrarTitulo("HISTORIAL DE CONVERSIONES (" + historial.size() + ")");
        if (historial.isEmpty()) {
            mensajes.mostrarAviso("  Aun no hay conversiones registradas en el servidor.");
            return;
        }

        mensajes.mostrarDetalle("  #    Fecha y hora         Tipo         Conversion");
        for (int indice = 0; indice < historial.size(); indice++) {
            RegistroConversion registro = historial.get(indice);
            mensajes.mostrarMensaje(String.format("  %-4d %s  %-12s %s", indice + 1,
                    FormatoUtil.formatearFechaHora(registro.fechaHora()),
                    registro.tipo().getNombreVisible(),
                    FormatoUtil.formatearConversion(registro)));
        }
        mensajes.mostrarDetalle("  Incluye las conversiones de todos los clientes conectados a este servidor.");
    }

    public void borrar() {
        mensajes.mostrarTitulo("BORRAR HISTORIAL");
        mensajes.mostrarAviso("  Se borraran las conversiones de todos los clientes conectados a este servidor.");
        if (!EntradaUtil.leerSiNo("Desea borrar el historial?")) {
            mensajes.mostrarAviso("  No se borro el historial.");
            return;
        }
        mensajes.mostrarExito(conversionControlador.limpiarHistorial());
    }
}
