package ec.edu.monster.conversion;

import ec.edu.monster.catalog.CatalogoUnidades;

/**
 * RF-05: Motor de conversión lineal de Masa (Base: kilogramos).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ConversorMasa {

    // Factores respecto al kilogramo
    private static final double FACTOR_GRAMOS     = 0.001;
    private static final double FACTOR_KILOGRAMOS = 1.0;
    private static final double FACTOR_ONZAS      = 0.0283495;
    private static final double FACTOR_LIBRAS     = 0.453592;
    private static final double FACTOR_QUINTALES  = 45.3592; // 1 quintal = 100 libras

    public double convertir(double valor, String unidadOrigen, String unidadDestino) {
        String origen = CatalogoUnidades.normalizar(unidadOrigen);
        String destino = CatalogoUnidades.normalizar(unidadDestino);

        double factorOrigen = obtenerFactor(origen);
        double factorDestino = obtenerFactor(destino);

        if (factorOrigen <= 0 || factorDestino <= 0) {
            throw new IllegalArgumentException("Unidad de masa no soportada: " + unidadOrigen + " o " + unidadDestino);
        }

        // Convertir a unidad base (kilogramos) y luego a la unidad destino
        double valorEnKilos = valor * factorOrigen;
        return valorEnKilos / factorDestino;
    }

    private double obtenerFactor(String unidad) {
        switch (unidad) {
            case "gramos":     return FACTOR_GRAMOS;
            case "kilogramos": return FACTOR_KILOGRAMOS;
            case "onzas":      return FACTOR_ONZAS;
            case "libras":     return FACTOR_LIBRAS;
            case "quintales":  return FACTOR_QUINTALES;
            default:           return -1.0;
        }
    }
}
