package ec.edu.monster.pruebas;

import ec.edu.monster.modelos.utilidades.ConversorLongitud;
import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.utilidades.enums.UnidadLongitud;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class ConversorLongitudTest {

    private final ConversorLongitud conversor = new ConversorLongitud();

    @ParameterizedTest
    @CsvSource({
        "1000, METRO, KILOMETRO, 1.0",
        "1, KILOMETRO, METRO, 1000.0",
        "100, CENTIMETRO, METRO, 1.0",
        "1000, MILIMETRO, METRO, 1.0",
        "1, METRO, MILIMETRO, 1000.0",
        "1, YARDA, METRO, 0.9144",
        "1, KILOMETRO, YARDA, 1093.6133",
        "91.44, CENTIMETRO, YARDA, 1.0",
        "5, METRO, METRO, 5.0",
        "0, KILOMETRO, MILIMETRO, 0.0"
    })
    void deberiaConvertirCorrectamente(double valor,
                                       UnidadLongitud origen,
                                       UnidadLongitud destino,
                                       double esperado) {

        double resultado = conversor.convertir(valor, origen, destino);

        assertEquals(esperado, resultado, 0.0001);
    }

    @ParameterizedTest
    @CsvSource({
        "-1, METRO, KILOMETRO",
        "-0.5, YARDA, METRO",
        "-1000, MILIMETRO, MILIMETRO"
    })
    void deberiaRechazarLongitudNegativa(double valor,
                                         UnidadLongitud origen,
                                         UnidadLongitud destino) {

        assertThrows(ExcepcionValidacion.class, () -> conversor.convertir(valor, origen, destino));
    }
}
