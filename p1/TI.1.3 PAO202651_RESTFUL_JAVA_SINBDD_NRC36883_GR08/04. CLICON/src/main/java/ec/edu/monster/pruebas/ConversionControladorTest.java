package ec.edu.monster.pruebas;

import ec.edu.monster.controlador.ConversionControlador;
import ec.edu.monster.modelo.ConversionModelo;
import ec.edu.monster.modelo.TipoConversion;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class ConversionControladorTest {

    private final ContratoFalso clienteWs = new ContratoFalso();
    private final ConversionControlador controlador = new ConversionControlador(clienteWs);

    @ParameterizedTest
    @CsvSource({
        "LONGITUD, 10, METRO, YARDA, convertirLongitud",
        "MASA, 2.5, KILOGRAMO, ONZA, convertirMasa",
        "TEMPERATURA, 25, CELSIUS, RANKINE, convertirTemperatura"
    })
    void deberiaLlamarLaOperacionDelTipoSeleccionado(TipoConversion tipo,
                                                     double valor,
                                                     String unidadInicial,
                                                     String unidadFinal,
                                                     String operacionEsperada) {

        double resultado = controlador.convertir(new ConversionModelo(tipo, valor, unidadInicial, unidadFinal));

        assertEquals(42.0, resultado, 0.000001);
        assertEquals(operacionEsperada, clienteWs.operacionLlamada);
        assertEquals(valor, clienteWs.valorRecibido, 0.000001);
        assertEquals(unidadInicial, clienteWs.unidadInicialRecibida);
        assertEquals(unidadFinal, clienteWs.unidadFinalRecibida);
    }

    @ParameterizedTest
    @CsvSource({
        "LONGITUD, longitud",
        "MASA, masa",
        "TEMPERATURA, temperatura"
    })
    void deberiaConsultarUnidadesPorCategoria(TipoConversion tipo, String categoria) {

        controlador.obtenerUnidades(tipo);

        assertEquals("consultarUnidades:" + categoria, clienteWs.operacionLlamada);
    }

    @ParameterizedTest
    @CsvSource({
        "LONGITUD, -1",
        "LONGITUD, -0.001",
        "MASA, -5",
        "MASA, -28.3495"
    })
    void deberiaRechazarLongitudYMasaNegativas(TipoConversion tipo, double valor) {
        assertThrows(IllegalArgumentException.class, () -> controlador.validarValor(tipo, valor));
        assertThrows(IllegalArgumentException.class,
                () -> controlador.convertir(new ConversionModelo(tipo, valor, "A", "B")));

        assertNull(clienteWs.operacionLlamada);
    }

    @ParameterizedTest
    @CsvSource({
        "LONGITUD, 0",
        "MASA, 0",
        "TEMPERATURA, -40",
        "TEMPERATURA, -273.15"
    })
    void deberiaAceptarCeroYTemperaturasNegativas(TipoConversion tipo, double valor) {
        assertDoesNotThrow(() -> controlador.validarValor(tipo, valor));
    }

    @ParameterizedTest
    @CsvSource({
        "NaN",
        "Infinity",
        "-Infinity"
    })
    void deberiaRechazarValoresNoFinitos(double valor) {
        assertThrows(IllegalArgumentException.class,
                () -> controlador.validarValor(TipoConversion.TEMPERATURA, valor));
    }

    @ParameterizedTest
    @CsvSource({
        "METRO, METRO",
        "metro, METRO",
        "CELSIUS, CELSIUS"
    })
    void deberiaRechazarUnidadesIguales(String unidadInicial, String unidadFinal) {
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> controlador.validarUnidades(unidadInicial, unidadFinal));

        assertEquals("La unidad final debe ser diferente a la unidad inicial.", ex.getMessage());
    }

    @Test
    void deberiaFallarCuandoSolicitudEsNula() {
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> controlador.convertir(null));

        assertEquals("La solicitud de conversion es obligatoria.", ex.getMessage());
        assertNull(clienteWs.operacionLlamada);
    }

    @Test
    void deberiaFallarCuandoTipoEsNulo() {
        ConversionModelo modelo = new ConversionModelo(null, 10.0, "METRO", "YARDA");

        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> controlador.convertir(modelo));

        assertEquals("Debe seleccionar el tipo de conversion.", ex.getMessage());
        assertNull(clienteWs.operacionLlamada);
    }

    @ParameterizedTest
    @CsvSource(value = {
        "NULL, YARDA, La unidad inicial es obligatoria.",
        "'   ', YARDA, La unidad inicial es obligatoria.",
        "METRO, NULL, La unidad final es obligatoria.",
        "METRO, '   ', La unidad final es obligatoria."
    }, nullValues = "NULL")
    void deberiaFallarCuandoFaltaUnaUnidad(String unidadInicial, String unidadFinal, String mensajeEsperado) {
        ConversionModelo modelo = new ConversionModelo(TipoConversion.LONGITUD, 10.0, unidadInicial, unidadFinal);

        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> controlador.convertir(modelo));

        assertEquals(mensajeEsperado, ex.getMessage());
        assertNull(clienteWs.operacionLlamada);
    }
}
