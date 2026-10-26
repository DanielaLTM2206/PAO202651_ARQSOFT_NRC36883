package ec.edu.monster.pruebas;

import ec.edu.monster.util.EntradaUtil;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.junit.jupiter.params.provider.ValueSource;

import static org.junit.jupiter.api.Assertions.*;

public class EntradaUtilTest {

    @ParameterizedTest
    @CsvSource(delimiter = '|', value = {
        "25|25.0",
        "12.5|12.5",
        "12,5|12.5",
        "-40|-40.0",
        "+3|3.0",
        "  7.25  |7.25",
        "0|0.0",
        "0.0000000001|0.0000000001",
        "999999999999999|999999999999999.0"
    })
    void deberiaAceptarNumerosDecimalesValidos(String texto, double esperado) {
        assertEquals(esperado, EntradaUtil.interpretarDecimal(texto), 0.0);
    }

    @ParameterizedTest
    @ValueSource(strings = {
        "", "   ", "abc", "12abc", "1e5", "1E5", "NaN", "Infinity", "-Infinity",
        "5d", "5f", "0x1p3", "1.2.3", "1,000.5", ".5", "5.", "--5", "1 000",
        "1234567890123456", "1.12345678901", "٣"
    })
    void deberiaRechazarTextoYFormatosNoNumericos(String texto) {
        assertThrows(IllegalArgumentException.class, () -> EntradaUtil.interpretarDecimal(texto));
    }
}
