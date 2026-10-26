package ec.edu.monster.conuniclientemovil.model;

/**
 * Mapea los códigos de error numéricos retornados por los servicios SOAP
 * a mensajes legibles para el usuario.
 *
 * Forma parte de la capa Model porque interpreta datos de dominio.
 */
public final class SoapErrorMapper {

    private static final double EPSILON = 0.0001;

    private SoapErrorMapper() {
    }

    /**
     * Verifica si un resultado de conversión representa un código de error del servidor.
     *
     * @param serviceType tipo de servicio invocado
     * @param value       valor retornado por el servicio
     * @return mensaje de error si el valor es un código de error; null en caso contrario
     */
    public static String mapConversionError(ServiceType serviceType, double value) {
        if (isClose(value, -999999.404)) {
            return "Unidad o escala invalida.";
        }

        if (isClose(value, -999999.500)) {
            return "Error interno de calculo en el servidor.";
        }

        if (serviceType == ServiceType.LONGITUD && isClose(value, -999999.401)) {
            return "Token invalido o expirado. Inicie sesion otra vez.";
        }

        if ((serviceType == ServiceType.MASA || serviceType == ServiceType.TEMPERATURA)
                && isClose(value, -9999999.401)) {
            return "Token invalido o expirado. Inicie sesion otra vez.";
        }

        return null;
    }

    private static boolean isClose(double left, double right) {
        return Math.abs(left - right) < EPSILON;
    }
}
