package ec.edu.monster.modelos.dto;

import jakarta.json.bind.annotation.JsonbProperty;

/**
 * Respuesta con un único mensaje. Se usa tanto para confirmaciones como para errores.
 */
public class MensajeRespuesta {

    @JsonbProperty("Mensaje")
    private String mensaje;

    public MensajeRespuesta() {}

    public MensajeRespuesta(String mensaje) {
        this.mensaje = mensaje;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String mensaje) {
        this.mensaje = mensaje;
    }
}
