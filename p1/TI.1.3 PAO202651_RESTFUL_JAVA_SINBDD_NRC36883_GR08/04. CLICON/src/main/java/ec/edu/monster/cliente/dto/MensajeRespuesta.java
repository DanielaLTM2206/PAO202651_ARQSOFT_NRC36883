package ec.edu.monster.cliente.dto;

import com.google.gson.annotations.SerializedName;

/**
 * Respuesta del servidor con un único mensaje: confirmaciones y errores (400, 401, 500).
 */
public class MensajeRespuesta {

    @SerializedName("Mensaje")
    private String mensaje;

    public MensajeRespuesta() {
    }

    public String getMensaje() {
        return mensaje;
    }
}
