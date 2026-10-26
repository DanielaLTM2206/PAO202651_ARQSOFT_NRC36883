package ec.edu.monster.excepciones;

/**
 * Datos de entrada faltantes, inválidos o inconsistentes. Se responde con HTTP 400.
 */
public class ExcepcionValidacion extends IllegalArgumentException {

    public ExcepcionValidacion(String mensaje) {
        super(mensaje);
    }
}
