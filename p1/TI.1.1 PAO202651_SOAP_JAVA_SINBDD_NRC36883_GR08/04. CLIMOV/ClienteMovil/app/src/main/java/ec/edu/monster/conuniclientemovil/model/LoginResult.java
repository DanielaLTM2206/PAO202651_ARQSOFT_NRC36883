package ec.edu.monster.conuniclientemovil.model;

/**
 * Modelo que encapsula el resultado estructurado de autenticación SOAP.
 */
public class LoginResult {

    private final boolean success;
    private final String token;
    private final String codigo;
    private final String errorMessage;

    public LoginResult(boolean success, String token, String codigo, String errorMessage) {
        this.success = success;
        this.token = token;
        this.codigo = codigo;
        this.errorMessage = errorMessage;
    }

    public static LoginResult success(String token) {
        return new LoginResult(true, token, "200", null);
    }

    public static LoginResult error(String codigo, String message) {
        return new LoginResult(false, null, codigo != null ? codigo : "AUTH-001", message);
    }

    public static LoginResult error(String message) {
        return new LoginResult(false, null, "AUTH-001", message);
    }

    public boolean isSuccess() {
        return success;
    }

    public String getToken() {
        return token;
    }

    public String getCodigo() {
        return codigo;
    }

    public String getErrorMessage() {
        return errorMessage;
    }
}
