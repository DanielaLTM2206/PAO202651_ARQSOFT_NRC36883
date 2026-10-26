package ec.edu.monster.util;

import java.util.Scanner;
import java.util.regex.Pattern;

public final class EntradaUtil {

    private static final Scanner ENTRADA = new Scanner(System.in);
    private static final Pattern PATRON_ENTERO = Pattern.compile("\\d{1,9}");
    private static final Pattern PATRON_DECIMAL = Pattern.compile("[+-]?\\d{1,15}([.,]\\d{1,10})?");

    private EntradaUtil() {
    }

    public static String leerTextoNoVacio(String mensaje) {
        while (true) {
            System.out.print(mensaje);
            String texto = ENTRADA.nextLine().trim();
            if (!texto.isEmpty()) {
                return texto;
            }
            System.out.println("  [ERROR] Entrada invalida. El valor no puede quedar vacio.");
        }
    }

    public static int leerEntero(String mensaje) {
        while (true) {
            System.out.print(mensaje);
            String valor = ENTRADA.nextLine().trim();
            if (PATRON_ENTERO.matcher(valor).matches()) {
                return Integer.parseInt(valor);
            }
            System.out.println("  [ERROR] Entrada invalida. Debe ingresar un numero entero.");
        }
    }

    public static double leerDecimal(String mensaje) {
        while (true) {
            System.out.print(mensaje);
            String valor = ENTRADA.nextLine();
            try {
                return interpretarDecimal(valor);
            } catch (IllegalArgumentException ex) {
                System.out.println("  [ERROR] " + ex.getMessage());
            }
        }
    }

    /**
     * Acepta solo números decimales simples: signo opcional, hasta 15 dígitos enteros y
     * hasta 10 decimales, separados por punto o coma (por ejemplo -12,5 o 1000.25).
     * Rechaza texto, notación científica (1e5), NaN, Infinity y sufijos de Java (5d, 5f).
     */
    public static double interpretarDecimal(String texto) {
        String valor = texto == null ? "" : texto.trim();

        if (valor.isEmpty()) {
            throw new IllegalArgumentException("Entrada invalida. El valor no puede quedar vacio.");
        }
        if (!PATRON_DECIMAL.matcher(valor).matches()) {
            throw new IllegalArgumentException(
                    "Entrada invalida. Ingrese solo numeros (maximo 15 enteros y 10 decimales), por ejemplo 25 o 12.5");
        }
        return Double.parseDouble(valor.replace(',', '.'));
    }

    public static boolean leerSiNo(String mensaje) {
        while (true) {
            String respuesta = leerTextoNoVacio(mensaje + " (S/N): ").toUpperCase();
            if (respuesta.equals("S")) {
                return true;
            }
            if (respuesta.equals("N")) {
                return false;
            }
            System.out.println("  [ERROR] Responda S o N.");
        }
    }
}
