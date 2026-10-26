package ec.edu.monster.pruebas;

import ec.edu.monster.modelos.utilidades.ConversorTemperatura;
import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.utilidades.enums.UnidadTemperatura;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.junit.jupiter.api.Assertions.*;

public class ConversorTemperaturaTest {

    private final ConversorTemperatura conversor = new ConversorTemperatura();

    @ParameterizedTest
    @CsvSource({
        "0, CELSIUS, FAHRENHEIT, 32.0",
        "32, FAHRENHEIT, CELSIUS, 0.0",
        "0, CELSIUS, KELVIN, 273.15",
        "273.15, KELVIN, CELSIUS, 0.0",
        "0, CELSIUS, RANKINE, 491.67",
        "491.67, RANKINE, CELSIUS, 0.0",
        "32, FAHRENHEIT, RANKINE, 491.67",
        "491.67, RANKINE, FAHRENHEIT, 32.0",
        "273.15, KELVIN, RANKINE, 491.67",
        "491.67, RANKINE, KELVIN, 273.15",
        "100, CELSIUS, FAHRENHEIT, 212.0",
        "-40, CELSIUS, FAHRENHEIT, -40.0",
        "212, FAHRENHEIT, KELVIN, 373.15",
        "0, KELVIN, FAHRENHEIT, -459.67",
        "25, CELSIUS, CELSIUS, 25.0",
        "-273.15, CELSIUS, KELVIN, 0.0"
    })
    void deberiaConvertirTemperatura(double valor,
                                     UnidadTemperatura origen,
                                     UnidadTemperatura destino,
                                     double esperado) {

        double resultado = conversor.convertir(valor, origen, destino);

        assertEquals(esperado, resultado, 0.0001);
    }

    @ParameterizedTest
    @CsvSource({
        "-1, KELVIN, CELSIUS",
        "-0.5, RANKINE, KELVIN",
        "-300, CELSIUS, FAHRENHEIT",
        "-500, FAHRENHEIT, CELSIUS",
        "-10, KELVIN, KELVIN"
    })
    void deberiaRechazarTemperaturaBajoCeroAbsoluto(double valor,
                                                    UnidadTemperatura origen,
                                                    UnidadTemperatura destino) {

        assertThrows(ExcepcionValidacion.class, () -> conversor.convertir(valor, origen, destino));
    }
}
