package ec.edu.monster.conversion;

import ec.edu.monster.catalog.CatalogoUnidades;

/**
 * RF-05: Motor de conversión no lineal de Temperatura (Base: Celsius).
 * Fórmulas matemáticas directas:
 * - Celsius, Fahrenheit, Kelvin, Newton, Reaumur.
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ConversorTemperatura {

    public double convertir(double valor, String unidadOrigen, String unidadDestino) {
        String origen = CatalogoUnidades.normalizar(unidadOrigen);
        String destino = CatalogoUnidades.normalizar(unidadDestino);

        if (origen.equals(destino)) {
            return valor;
        }

        // 1. Llevar cualquier temperatura a Celsius
        double celsius;
        switch (origen) {
            case "celsius":
                celsius = valor;
                break;
            case "fahrenheit":
                celsius = (valor - 32.0) * (5.0 / 9.0);
                break;
            case "kelvin":
                celsius = valor - 273.15;
                break;
            case "newton":
                celsius = valor * (100.0 / 33.0);
                break;
            case "reaumur":
                celsius = valor * (5.0 / 4.0);
                break;
            default:
                throw new IllegalArgumentException("Unidad de temperatura origen no soportada: " + unidadOrigen);
        }

        // 2. Llevar de Celsius a la unidad destino
        switch (destino) {
            case "celsius":
                return celsius;
            case "fahrenheit":
                return (celsius * (9.0 / 5.0)) + 32.0;
            case "kelvin":
                return celsius + 273.15;
            case "newton":
                return celsius * (33.0 / 100.0);
            case "reaumur":
                return celsius * (4.0 / 5.0);
            default:
                throw new IllegalArgumentException("Unidad de temperatura destino no soportada: " + unidadDestino);
        }
    }
}
