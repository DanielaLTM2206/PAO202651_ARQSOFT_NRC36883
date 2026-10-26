package ec.edu.monster.conuniclientemovil.model;

/**
 * Modelo que encapsula la Respuesta Estructurada (RF-06 / CU-07) de una conversión SOAP.
 */
public class ConversionResult {

    private final String estado;
    private final String codigo;
    private final String mensaje;
    private final Double resultado;

    public ConversionResult(String estado, String codigo, String mensaje, Double resultado) {
        this.estado = estado;
        this.codigo = codigo;
        this.mensaje = mensaje;
        this.resultado = resultado;
    }

    public static ConversionResult success(String codigo, String mensaje, double resultado) {
        return new ConversionResult("OK", codigo != null ? codigo : "200", mensaje, resultado);
    }

    public static ConversionResult error(String codigo, String mensaje) {
        return new ConversionResult("ERROR", codigo != null ? codigo : "ERR", mensaje, null);
    }

    public String getEstado() {
        return estado;
    }

    public String getCodigo() {
        return codigo;
    }

    public String getMensaje() {
        return mensaje;
    }

    public Double getResultado() {
        return resultado;
    }

    public boolean isSuccess() {
        return "OK".equalsIgnoreCase(estado);
    }

    public boolean isTokenError() {
        return "AUTH-001".equalsIgnoreCase(codigo) ||
               (mensaje != null && mensaje.toLowerCase().contains("token"));
    }
}
