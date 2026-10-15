package ec.edu.monster.conversion;

import ec.edu.monster.catalog.CatalogoUnidades;

/**
 * RF-05: Motor de conversión lineal de Longitud (Base: metros).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ConversorLongitud {

    // Factores respecto al metro
    private static final double FACTOR_CENTIMETROS = 0.01;
    private static final double FACTOR_METROS      = 1.0;
    private static final double FACTOR_PIES        = 0.3048;
    private static final double FACTOR_YARDAS      = 0.9144;
    private static final double FACTOR_MILLAS      = 1609.34;

    public double convertir(double valor, String unidadOrigen, String unidadDestino) {
        String origen = CatalogoUnidades.normalizar(unidadOrigen);
        String destino = CatalogoUnidades.normalizar(unidadDestino);

        double factorOrigen = obtenerFactor(origen);
        double factorDestino = obtenerFactor(destino);

        if (factorOrigen <= 0 || factorDestino <= 0) {
            throw new IllegalArgumentException("Unidad de longitud no soportada: " + unidadOrigen + " o " + unidadDestino);
        }

        // Convertir a unidad base (metros) y luego a la unidad destino
        double valorEnMetros = valor * factorOrigen;
        return valorEnMetros / factorDestino;
    }

    private double obtenerFactor(String unidad) {
        switch (unidad) {
            case "centimetros": return FACTOR_CENTIMETROS;
            case "metros":      return FACTOR_METROS;
            case "pies":        return FACTOR_PIES;
            case "yardas":      return FACTOR_YARDAS;
            case "millas":      return FACTOR_MILLAS;
            default:            return -1.0;
        }
    }
}
