package ec.edu.monster.security;

/**
 * Resultado de la validación de un token de seguridad (CU-05, CU-08, CU-09).
 * 
 * @author GR08 - Arquitectura de Software
 */
public class ResultadoValidacionToken {

    private final boolean valido;
    private final String codigo;
    private final String mensaje;

    private ResultadoValidacionToken(boolean valido, String codigo, String mensaje) {
        this.valido = valido;
        this.codigo = codigo;
        this.mensaje = mensaje;
    }

    public static ResultadoValidacionToken exitoso() {
        return new ResultadoValidacionToken(true, "200", "Token válido");
    }

    public static ResultadoValidacionToken invalido(String mensaje) {
        return new ResultadoValidacionToken(false, "AUTH-001", mensaje);
    }

    public static ResultadoValidacionToken expirado(String mensaje) {
        return new ResultadoValidacionToken(false, "AUTH-001", mensaje);
    }

    public boolean isValido() {
        return valido;
    }

    public String getCodigo() {
        return codigo;
    }

    public String getMensaje() {
        return mensaje;
    }
}
