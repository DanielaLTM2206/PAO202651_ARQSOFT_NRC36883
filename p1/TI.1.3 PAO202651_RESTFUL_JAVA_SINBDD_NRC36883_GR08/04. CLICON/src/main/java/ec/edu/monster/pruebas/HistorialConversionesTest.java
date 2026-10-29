package ec.edu.monster.pruebas;

import ec.edu.monster.cliente.dto.RegistroConversionRespuesta;
import ec.edu.monster.controlador.ConversionControlador;
import ec.edu.monster.modelo.RegistroConversion;
import ec.edu.monster.modelo.TipoConversion;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.time.LocalDateTime;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

/**
 * El historial es compartido: lo guarda el servidor y el controlador solo lo consulta o lo borra.
 */
public class HistorialConversionesTest {

    private final ContratoFalso clienteWs = new ContratoFalso();
    private final ConversionControlador controlador = new ConversionControlador(clienteWs);

    @Test
    void deberiaIniciarVacioSiElServidorNoTieneConversiones() {
        assertTrue(controlador.obtenerHistorial().isEmpty());
        assertEquals("consultarHistorial", clienteWs.operacionLlamada);
    }

    @Test
    void deberiaConvertirLosRegistrosDelServidorAlModelo() {
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:28:36", "Longitud", 1000, "METRO", 1, "KILOMETRO"));

        List<RegistroConversion> historial = controlador.obtenerHistorial();

        assertEquals(List.of(new RegistroConversion(LocalDateTime.of(2026, 10, 18, 19, 28, 36),
                TipoConversion.LONGITUD, 1000, "METRO", 1, "KILOMETRO")), historial);
    }

    @Test
    void deberiaAceptarHorasSinSegundos() {
        // LocalDateTime.toString() omite los segundos cuando valen cero (2026-10-18T19:28)
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:28", "Masa", 1, "GRAMO", 0.001, "KILOGRAMO"));

        RegistroConversion registro = controlador.obtenerHistorial().get(0);

        assertEquals(LocalDateTime.of(2026, 10, 18, 19, 28, 0), registro.fechaHora());
    }

    @Test
    void deberiaConservarElOrdenDelServidor() {
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:30:00", "Temperatura", 3, "CELSIUS", 276.15, "KELVIN"));
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:29:00", "Masa", 2, "GRAMO", 0.07, "ONZA"));
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:28:00", "Longitud", 1, "METRO", 1.09, "YARDA"));

        List<TipoConversion> tipos = controlador.obtenerHistorial().stream().map(RegistroConversion::tipo).toList();

        assertEquals(List.of(TipoConversion.TEMPERATURA, TipoConversion.MASA, TipoConversion.LONGITUD), tipos);
    }

    @ParameterizedTest
    @CsvSource(value = {
        "no-es-fecha, Longitud, METRO, KILOMETRO",
        "2026-10-18T19:28:36, Volumen, METRO, KILOMETRO",
        "2026-10-18T19:28:36, Longitud, '', KILOMETRO",
        "2026-10-18T19:28:36, Longitud, METRO, ''",
        "NULL, Longitud, METRO, KILOMETRO"
    }, nullValues = "NULL")
    void deberiaOmitirRegistrosIncompletos(String fecha, String tipo, String origen, String destino) {
        clienteWs.historialServidor.add(new RegistroConversionRespuesta(fecha, tipo, 1, origen, 1, destino));
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:28:36", "Masa", 1, "GRAMO", 0.001, "KILOGRAMO"));

        List<RegistroConversion> historial = controlador.obtenerHistorial();

        assertEquals(1, historial.size());
        assertEquals(TipoConversion.MASA, historial.get(0).tipo());
    }

    @Test
    void deberiaBorrarElHistorialEnElServidor() {
        clienteWs.historialServidor.add(
                new RegistroConversionRespuesta("2026-10-18T19:28:36", "Longitud", 1, "METRO", 100, "CENTIMETRO"));

        String mensaje = controlador.limpiarHistorial();

        assertEquals("limpiarHistorial", clienteWs.operacionLlamada);
        assertEquals("Historial de conversiones borrado", mensaje);
        assertTrue(controlador.obtenerHistorial().isEmpty());
    }
}
