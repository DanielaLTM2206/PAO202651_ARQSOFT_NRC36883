package ec.edu.monster.util;

import ec.edu.monster.modelo.RegistroConversion;

import java.math.BigDecimal;
import java.math.MathContext;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public final class FormatoUtil {

    // 10 cifras significativas: suficiente precisión sin arrastrar errores de punto flotante
    private static final MathContext PRECISION = new MathContext(10);
    private static final DateTimeFormatter FORMATO_FECHA_HORA = DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm:ss");

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

    public static String formatearFechaHora(LocalDateTime fechaHora) {
        return fechaHora.format(FORMATO_FECHA_HORA);
    }

    // Por ejemplo: 1000 METRO = 1 KILOMETRO
    public static String formatearConversion(RegistroConversion registro) {
        return formatearNumero(registro.valor()) + " " + registro.unidadOrigen() + " = "
                + formatearNumero(registro.resultado()) + " " + registro.unidadDestino();
    }
}
