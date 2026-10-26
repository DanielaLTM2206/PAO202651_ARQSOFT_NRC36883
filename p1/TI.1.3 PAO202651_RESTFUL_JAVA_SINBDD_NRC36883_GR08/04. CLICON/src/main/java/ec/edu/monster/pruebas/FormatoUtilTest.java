package ec.edu.monster.pruebas;

import ec.edu.monster.util.FormatoUtil;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class FormatoUtilTest {

    @ParameterizedTest
    @CsvSource({
        "1.0, 1",
        "212.0, 212",
        "28.3495, 28.3495",
        "0.000000001, 0.000000001",
        "1093.6132983377078, 1093.613298",
        "0.30000000000000004, 0.3",
        "-459.67, -459.67",
        "0.0, 0",
        "1000000000000.0, 1000000000000"
    })
    void deberiaMostrarNumerosSinNotacionCientificaNiCerosSobrantes(double numero, String esperado) {
        assertEquals(esperado, FormatoUtil.formatearNumero(numero));
    }
}
