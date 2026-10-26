package ec.edu.monster.excepciones;

/**
 * Credenciales incorrectas o token ausente, inválido o expirado. Se responde con HTTP 401.
 */
public class ExcepcionAutenticacion extends RuntimeException {

    public ExcepcionAutenticacion(String mensaje) {
        super(mensaje);
    }
}
