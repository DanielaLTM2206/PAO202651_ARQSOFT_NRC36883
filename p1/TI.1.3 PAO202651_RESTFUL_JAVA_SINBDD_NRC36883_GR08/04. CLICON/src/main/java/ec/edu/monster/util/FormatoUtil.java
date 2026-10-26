package ec.edu.monster.util;

import java.math.BigDecimal;
import java.math.MathContext;

public final class FormatoUtil {

    // 10 cifras significativas: suficiente precisión sin arrastrar errores de punto flotante
    private static final MathContext PRECISION = new MathContext(10);

    private FormatoUtil() {
    }

    /**
     * Muestra el número sin notación científica ni ceros sobrantes,
     * de modo que valores muy pequeños (0.000000001) no se vean como 0.
     */
    public static String formatearNumero(double numero) {
        return BigDecimal.valueOf(numero)
                .round(PRECISION)
                .stripTrailingZeros()
                .toPlainString();
    }
}
