package ec.edu.monster.pruebas;

import ec.edu.monster.modelos.utilidades.ConversorMasa;
import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.utilidades.enums.UnidadMasa;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class ConversorMasaTest {

    private final ConversorMasa conversor = new ConversorMasa();

    @ParameterizedTest
    @CsvSource({
        "1000, GRAMO, KILOGRAMO, 1.0",
        "1, KILOGRAMO, GRAMO, 1000.0",
        "500, GRAMO, KILOGRAMO, 0.5",
        "1000, MILIGRAMO, GRAMO, 1.0",
        "1, TONELADA, KILOGRAMO, 1000.0",
        "2500, KILOGRAMO, TONELADA, 2.5",
        "1, ONZA, GRAMO, 28.3495",
        "28.3495, GRAMO, ONZA, 1.0",
        "7, GRAMO, GRAMO, 7.0"
    })
    void deberiaConvertirCorrectamente(double valor,
                                       UnidadMasa origen,
                                       UnidadMasa destino,
                                       double esperado) {

        double resultado = conversor.convertir(valor, origen, destino);

        assertEquals(esperado, resultado, 0.0001);
    }

    @ParameterizedTest
    @CsvSource({
        "-1, GRAMO, KILOGRAMO",
        "-28.3495, ONZA, GRAMO",
        "-0.001, TONELADA, TONELADA"
    })
    void deberiaRechazarMasaNegativa(double valor,
                                     UnidadMasa origen,
                                     UnidadMasa destino) {

        assertThrows(ExcepcionValidacion.class, () -> conversor.convertir(valor, origen, destino));
    }
}
