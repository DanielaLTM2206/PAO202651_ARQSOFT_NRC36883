package ec.edu.monster.pruebas;

import ec.edu.monster.modelos.dto.RegistroConversionRespuesta;
import ec.edu.monster.servicios.ServicioHistorial;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import java.time.LocalDateTime;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

public class ServicioHistorialTest {

    private static final LocalDateTime FECHA = LocalDateTime.of(2026, 10, 18, 19, 28, 36, 123_000_000);

    @BeforeEach
    void limpiarHistorial() {
        ServicioHistorial.limpiar();
    }

    @Test
    void deberiaIniciarVacio() {
        assertTrue(ServicioHistorial.obtener().isEmpty());
    }

    @ParameterizedTest
    @CsvSource({
        "Longitud, 1000, METRO, 1, KILOMETRO",
        "Masa, 1, ONZA, 28.3495, GRAMO",
        "Temperatura, 100, CELSIUS, 212, FAHRENHEIT"
    })
    void deberiaRegistrarLaConversion(String tipo, double valor, String origen, double resultado, String destino) {
        ServicioHistorial.registrar(FECHA, tipo, valor, origen, resultado, destino);

        RegistroConversionRespuesta registro = ServicioHistorial.obtener().get(0);
        assertEquals("2026-10-18T19:28:36", registro.getFechaHora());
        assertEquals(tipo, registro.getTipo());
        assertEquals(valor, registro.getValor(), 0.000001);
        assertEquals(origen, registro.getUnidadOrigen());
        assertEquals(resultado, registro.getResultado(), 0.000001);
        assertEquals(destino, registro.getUnidadDestino());
    }

    @Test
    void deberiaMostrarPrimeroLaMasReciente() {
        ServicioHistorial.registrar(FECHA, "Longitud", 1, "METRO", 1.0936, "YARDA");
        ServicioHistorial.registrar(FECHA, "Masa", 2, "GRAMO", 0.0705, "ONZA");
        ServicioHistorial.registrar(FECHA, "Temperatura", 3, "CELSIUS", 276.15, "KELVIN");

        List<String> tipos = ServicioHistorial.obtener().stream().map(RegistroConversionRespuesta::getTipo).toList();
        assertEquals(List.of("Temperatura", "Masa", "Longitud"), tipos);
    }

    @Test
    void deberiaConservarSoloLosRegistrosMasRecientes() {
        for (int i = 1; i <= ServicioHistorial.CAPACIDAD_MAXIMA + 5; i++) {
            ServicioHistorial.registrar(FECHA, "Masa", i, "GRAMO", i, "KILOGRAMO");
        }

        List<RegistroConversionRespuesta> registros = ServicioHistorial.obtener();
        assertEquals(ServicioHistorial.CAPACIDAD_MAXIMA, registros.size());
        assertEquals(ServicioHistorial.CAPACIDAD_MAXIMA + 5, registros.get(0).getValor(), 0.0);
        assertEquals(6, registros.get(registros.size() - 1).getValor(), 0.0);
    }

    @Test
    void deberiaVaciarseAlLimpiar() {
        ServicioHistorial.registrar(FECHA, "Longitud", 1, "METRO", 100, "CENTIMETRO");

        ServicioHistorial.limpiar();

        assertTrue(ServicioHistorial.obtener().isEmpty());
    }

    @Test
    void laListaDevueltaNoDeberiaPermitirModificarElHistorial() {
        ServicioHistorial.registrar(FECHA, "Longitud", 1, "METRO", 100, "CENTIMETRO");

        assertThrows(UnsupportedOperationException.class, () -> ServicioHistorial.obtener().clear());
        assertEquals(1, ServicioHistorial.obtener().size());
    }
}
