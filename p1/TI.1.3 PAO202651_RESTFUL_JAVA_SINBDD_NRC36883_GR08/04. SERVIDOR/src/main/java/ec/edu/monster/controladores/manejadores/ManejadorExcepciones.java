package ec.edu.monster.controladores.manejadores;

import ec.edu.monster.excepciones.ExcepcionAutenticacion;
import ec.edu.monster.excepciones.ExcepcionValidacion;
import ec.edu.monster.modelos.dto.MensajeRespuesta;

import jakarta.json.bind.JsonbException;
import jakarta.ws.rs.ProcessingException;
import jakarta.ws.rs.WebApplicationException;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.ext.ExceptionMapper;
import jakarta.ws.rs.ext.Provider;

import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * Traduce cualquier excepción al JSON {"Mensaje": "..."} con su código HTTP.
 */
@Provider
public class ManejadorExcepciones implements ExceptionMapper<Throwable> {

    private static final Logger LOG = Logger.getLogger(ManejadorExcepciones.class.getName());

    @Override
    public Response toResponse(Throwable excepcion) {

        if (excepcion instanceof ExcepcionValidacion) {
            return construir(Response.Status.BAD_REQUEST.getStatusCode(), excepcion.getMessage());
        }
        if (excepcion instanceof ExcepcionAutenticacion) {
            return construir(Response.Status.UNAUTHORIZED.getStatusCode(), excepcion.getMessage());
        }
        // JSON mal formado o con tipos incorrectos (p. ej. "Valor": "abc")
        if (excepcion instanceof JsonbException || excepcion instanceof ProcessingException) {
            return construir(Response.Status.BAD_REQUEST.getStatusCode(),
                    "El cuerpo de la solicitud no es un JSON válido");
        }
        // Errores propios de JAX-RS (404, 405, 415...): se respeta su código original
        if (excepcion instanceof WebApplicationException webExcepcion) {
            int estado = webExcepcion.getResponse().getStatus();
            return construir(estado, mensajeHttp(estado));
        }

        LOG.log(Level.SEVERE, "Error no controlado", excepcion);
        return construir(Response.Status.INTERNAL_SERVER_ERROR.getStatusCode(), "Error interno del servidor");
    }

    private static String mensajeHttp(int estado) {
        return switch (estado) {
            case 400 -> "Solicitud inválida";
            case 404 -> "Recurso no encontrado";
            case 405 -> "Método HTTP no permitido";
            case 406 -> "Formato de respuesta no soportado, use application/json";
            case 415 -> "Tipo de contenido no soportado, use application/json";
            default -> estado >= 500 ? "Error interno del servidor" : "Error en la solicitud";
        };
    }

    private static Response construir(int estado, String mensaje) {
        return Response.status(estado)
                .type(MediaType.APPLICATION_JSON)
                .entity(new MensajeRespuesta(mensaje))
                .build();
    }
}
