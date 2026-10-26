package ec.edu.monster.pruebas;

import ec.edu.monster.modelo.TipoConversion;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class TipoConversionTest {

    @ParameterizedTest
    @CsvSource({
        "LONGITUD, Longitud, longitud",
        "MASA, Masa, masa",
        "TEMPERATURA, Temperatura, temperatura"
    })
    void deberiaExponerNombreVisibleYCategoria(TipoConversion tipo, String nombreVisible, String categoria) {
        assertEquals(nombreVisible, tipo.getNombreVisible());
        assertEquals(categoria, tipo.getCategoria());
    }
}
